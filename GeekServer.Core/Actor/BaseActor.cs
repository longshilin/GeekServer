using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Geek.Server
{
    public abstract class BaseActor
    {
        readonly static NLog.Logger LOGGER = NLog.LogManager.GetCurrentClassLogger();
        public const int TIME_OUT = 10000;

        public object Lockable = new object();
        /// <summary>
        /// 调用链 ---  正在等待的ActorId
        /// </summary>
        public static ConcurrentDictionary<long, BaseActor> WaitingMap = new ConcurrentDictionary<long, BaseActor>();
        /// <summary>
        /// 当前调用链id
        /// </summary>
        internal long curCallChainId;   
        private static long idCounter = 1;

        public long ActorId { get; set; }
        public BaseActor(int parallelism = 1)
        {
            actionBlock = new ActionBlock<WorkWrapper>(InnerRun, 
                new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = parallelism });
        }

        readonly ActionBlock<WorkWrapper> actionBlock;

        static async Task InnerRun(WorkWrapper wrapper)
        {
            var task = wrapper.DoTask();
            var res = await task.WaitAsync(TimeSpan.FromMilliseconds(wrapper.TimeOut));
            if (res)
            {
                LOGGER.Fatal("wrapper执行超时:" + wrapper.GetTrace());
                //强制设状态-取消该操作
                wrapper.ForceSetResult();
            }
        }

        private void IsNeedEqueue(bool allowCallChainReentrancy, out bool needEqueue, out long callChainId)
        {
            lock (Lockable)
            {
                callChainId = RuntimeContext.Current;
                if (!allowCallChainReentrancy)
                {
                    callChainId = Interlocked.Increment(ref idCounter);
                    needEqueue = true;
                    return;
                }

                if (callChainId <= 0)
                {
                    callChainId = Interlocked.Increment(ref idCounter);
                    needEqueue = true;
                }
                else if (callChainId == curCallChainId)
                {
                    needEqueue = false;
                    return;
                }

                if (curCallChainId > 0)
                {
                    WaitingMap.TryGetValue(curCallChainId, out var waiting);
                    if (waiting != null && waiting.curCallChainId == callChainId)
                    {
                        needEqueue = false;
                    }
                    else
                    {
                        WaitingMap[callChainId] = this;
                        needEqueue = true;
                    }
                }
                else
                {
                    needEqueue = true;
                }
            }
        }

        public Task SendAsync(Action work, bool allowCallChainReentrancy = true, int timeOut = TIME_OUT)
        {
            IsNeedEqueue(allowCallChainReentrancy, out bool needEqueue, out long callChainId);
            if (needEqueue)
            {
                ActionWrapper at = new ActionWrapper(work);
                at.Owner = this;
                at.TimeOut = timeOut;
                at.CallChainId = callChainId;
                actionBlock.SendAsync(at);
                return at.Tcs.Task;
            }
            else
            {
                work();
                return Task.CompletedTask;
            }
        }

        public Task<T> SendAsync<T>(Func<T> work, bool allowCallChainReentrancy = true, int timeOut = TIME_OUT)
        {
            IsNeedEqueue(allowCallChainReentrancy, out bool needEqueue, out long callChainId);
            if (needEqueue)
            {
                FuncWrapper<T> at = new FuncWrapper<T>(work);
                at.Owner = this;
                at.TimeOut = timeOut;
                at.CallChainId = callChainId;
                actionBlock.SendAsync(at);
                return at.Tcs.Task;
            }
            else
            {
                return Task.FromResult(work());
            }
        }

        public Task SendAsync(Func<Task> work, bool allowCallChainReentrancy = true, int timeOut = TIME_OUT)
        {
            IsNeedEqueue(allowCallChainReentrancy, out bool needEqueue, out long callChainId);
            if (needEqueue)
            {
                ActionAsyncWrapper at = new ActionAsyncWrapper(work);
                at.Owner = this;
                at.TimeOut = timeOut;
                at.CallChainId = callChainId;
                actionBlock.SendAsync(at);
                return at.Tcs.Task;
            }
            else
            {
                return work();
            }
        }

        public Task<T> SendAsync<T>(Func<Task<T>> work, bool allowCallChainReentrancy = true, int timeOut = TIME_OUT)
        {
            IsNeedEqueue(allowCallChainReentrancy, out bool needEqueue, out long callChainId);
            if (needEqueue)
            {
                FuncAsyncWrapper<T> at = new FuncAsyncWrapper<T>(work);
                at.Owner = this;
                at.TimeOut = timeOut;
                at.CallChainId = callChainId;
                actionBlock.SendAsync(at);
                return at.Tcs.Task;
            }
            else
            {
                return work();
            }
        }

        public abstract Task Active();

        public abstract Task Deactive();

        public virtual Task<bool> ReadyToDeactive()
        {
            return Task.FromResult(true);
        }
    }
}

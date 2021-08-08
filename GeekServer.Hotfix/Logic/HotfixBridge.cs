using System;
using Geek.Server.Config;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Geek.Server
{
    public class HotfixBridge : IHotfixBridge
    {
        static readonly NLog.Logger LOGGER = NLog.LogManager.GetCurrentClassLogger();

        public ServerType BridgeType => ServerType.Game;
        public async Task<bool> OnLoadSucceed(bool isReload)
        {
            try
            {
                if (isReload)
                {
                    //热更
                    LOGGER.Info("load配置表...");
                    //配置表代码在hotfix，热更后当前域的GameDataManager=null，需要重新加载配置表
                    (bool beanSuccess, string msg) = GameDataManager.ReloadAll();
                    if(!beanSuccess)
                    {
                        LOGGER.Info("load配置表异常...");
                        LOGGER.Error(msg);
                        return false;
                    }

                    LOGGER.Info("清除缓存的agent...");
                    await ActorManager.ActorsForeach((actor) =>
                    {
                        actor.SendAsync(actor.ClearCacheAgent, true);
                        return Task.CompletedTask;
                    });
                    LOGGER.Info("hotfix load success");
                }else
                {
                    //起服
                    if (!await Start())
                        return false;
                }
				
				//成功了才替换msg&handler
				HttpHandlerFactory.SetExtraHandlerGetter(HotfixMgr.GetHttpHandler);
                TcpHandlerFactory.SetExtraHandlerGetter(Geek.Server.Message.MsgFactory.Create, msgId => HotfixMgr.GetHandler<BaseTcpHandler>(msgId));
                return true;
            }
            catch(Exception e)
            {
                LOGGER.Fatal("OnLoadSucceed执行异常");
                LOGGER.Fatal(e.ToString());
                return false;
            }
        }

        async Task<bool> Start()
        {
            try
            {
                LOGGER.Info("start server......");
                await HttpServer.Start(Settings.Ins.HttpPort);
                await TcpServer.Start(Settings.Ins.TcpPort, Settings.Ins.UseLibuv);

                LOGGER.Info("init mongodb......" + Settings.Ins.MongoUrl);
                MongoDBConnection.Singleton.Connect(Settings.Ins.MongoDB, Settings.Ins.MongoUrl);

                LOGGER.Info("启动回存timer......");
                GlobalDBTimer.Singleton.Start();

                LOGGER.Info("注册所有组件......");
                ComponentTools.RegistAllComps();

                LOGGER.Info("load配置表...");
                (bool beanSuccess, string msg) = GameDataManager.ReloadAll();
                if (!beanSuccess)
                {
                    LOGGER.Error(msg);
                    return false;
                }

                LOGGER.Info("激活所有全局actor...");
                var taskList = new List<Task>();
                taskList.Add(activeActorAndItsComps<ServerActorAgent>(ServerActorID.GetID(ActorType.Normal)));
                taskList.Add(activeActorAndItsComps<SampleActorAgent>(ServerActorID.GetID(ActorType.Test)));
                //激活其他全局actor

                await Task.WhenAll(taskList);

                var serverActor = await ActorManager.GetOrNew<ServerActorAgent>(ServerActorID.GetID(ActorType.Normal));
                _ = serverActor.SendAsync(serverActor.CheckCrossDay, false);

                return true;
            }catch(Exception e)
            {
                LOGGER.Fatal("起服失败\n" + e.ToString());
                return false;
            }
        }

        async Task activeActorAndItsComps<TActorAgent>(long actorId) where TActorAgent : IComponentActorAgent
        {
            var actor = await ActorManager.GetOrNew<TActorAgent>(actorId);
            await ((ComponentActor)actor.Owner).ActiveAllComps();
        }

        public async Task<bool> Stop()
        {
            try
            {
                await QuartzTimer.Stop();
                await ChannelManager.RemoveAll();
                await GlobalDBTimer.Singleton.OnShutdown();
                await ActorManager.RemoveAll();
                await TcpServer.Stop();
                await HttpServer.Stop();
                return true;
            }catch(Exception e)
            {
                LOGGER.Fatal(e.ToString());
                return false;
            }
        }
    }
}

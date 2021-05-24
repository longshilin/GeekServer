namespace Geek.Server
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class ServerActor : ComponentActor
    {
        /// <summary>
        /// 服务器内部事件
        /// </summary>
        public readonly EventDispatcher EvtDispatcher;
        public ServerActor()
        {
            EvtDispatcher = new EventDispatcher(this);
        }
    }
}
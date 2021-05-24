namespace Geek.Server
{
    /// <summary>
    /// 玩家/玩家角色
    /// </summary>
    public class RoleActor : ComponentActor
    {
        /// <summary>内存中所有玩家事件（世界等级，开服天数。。。）</summary>
        public static readonly CrossActorEventDispatcher<RoleActor> AllRoleEvtDispatcher = new CrossActorEventDispatcher<RoleActor>();

        /// <summary>单个玩家事件(任务，登陆。。。)</summary>
        public readonly EventDispatcher EvtDispatcher;
        public RoleActor()
        {
            EvtDispatcher = new EventDispatcher(this);
        }
    }
}
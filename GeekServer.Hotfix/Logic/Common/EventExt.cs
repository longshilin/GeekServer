using System.Threading.Tasks;

namespace Geek.Server
{
    public static class EventExt
    {
        public static void AddListener<T>(this EventDispatcher dispatcher, EventID evtId) where T : IEventListener
        {
            dispatcher.AddListener((int)evtId, typeof(T).FullName);
        }

        public static void AddListener(this EventDispatcher dispatcher, EventID evtId, IEventListener listener)
        {
            dispatcher.AddListener((int)evtId, listener.GetType().FullName);
        }

        public static void RemoveListener<T>(this EventDispatcher dispatcher, EventID evtId) where T : IEventListener
        {
            dispatcher.RemoveListener((int)evtId, typeof(T).FullName);
        }

        public static void DispatchEvent(this EventDispatcher dispatcher, EventID evtId, Param param = null)
        {
            dispatcher.DispatchEvent((int)evtId, param);
        }




        public static void AddListener<T>(this CrossActorEventDispatcher<RoleActor> dispatcher, long roleId, EventID evtId) where T : IEventListener
        {
            dispatcher.AddListener(roleId, (int)evtId, typeof(T).FullName);
        }

        public static void AddListener(this CrossActorEventDispatcher<RoleActor> dispatcher, long roleId, EventID evtId, IEventListener listener)
        {
            dispatcher.AddListener(roleId, (int)evtId, listener.GetType().FullName);
        }

        public static void RemoveListener<T>(this CrossActorEventDispatcher<RoleActor> dispatcher, long roleId, EventID evtId) where T : IEventListener
        {
            dispatcher.RemoveListener(roleId, (int)evtId, typeof(T).FullName);
        }

        public static void ClearListener(this CrossActorEventDispatcher<RoleActor> dispatcher, long roleId)
        {
            dispatcher.ClearListener(roleId);
        }

        public static void DispatchEvent(this CrossActorEventDispatcher<RoleActor> dispatcher, EventID evtId, Param param = null)
        {
            dispatcher.DispatchEvent((int)evtId, checkRoleDispatch, param);
        }

        static Task<bool> checkRoleDispatch(int evtType, RoleActor actor, System.Type compType)
        {
            if (actor == null)
                return Task.FromResult(false);
            return Task.FromResult(true);
        }
    }
}
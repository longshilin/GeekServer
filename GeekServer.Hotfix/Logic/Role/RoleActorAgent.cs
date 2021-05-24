
namespace Geek.Server
{
    public class RoleActorAgent : ComponentActorAgent<RoleActor>
    {
        public EventDispatcher EvtDispatcher => Actor.EvtDispatcher;
    }
}
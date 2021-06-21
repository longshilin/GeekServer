using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    public abstract class DemoRoleActorHandler : TcpActorHandler
    {
        public Task<TAgent> GetCompAgent<TAgent>() where TAgent : IComponentAgent, new()
        {
            return Actor.GetAgentAs<DemoRoleActorAgent>().GetCompAgent<TAgent>();
        }

        public override async Task<ComponentActor> CacheActor()
        {
            var att = Ctx.GetAttribute<Channel>(ChannelManager.Att_Channel);
            var channel = att.Get();
            var agent = await ActorManager.GetOrNew<DemoRoleActorAgent>(channel.Id);
            return agent.Actor;
        }
    }
}

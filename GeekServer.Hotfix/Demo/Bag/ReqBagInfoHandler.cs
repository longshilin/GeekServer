using Geek.Server.Message.DemoBag;
using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    [TcpMsgMapping(typeof(ReqBagInfo))]
    public class ReqBagInfoHandler : DemoRoleActorHandler
    {
        public override async Task ActionAsync()
        {
            var bagComp = await GetCompAgent<DemoBagCompAgent>();
            var msg = await bagComp.BuildInfoMsg();
            WriteAndFlush(msg);
        }
    }
}

using System.Threading.Tasks;

namespace Geek.Server
{
    [TcpMsgMapping(typeof(Message.Sample.ReqTest))]
    public class SampleTcpHandler : BaseTcpHandler
    {
        public override Task ActionAsync()
        {
            var req = (Message.Sample.ReqTest)Msg;
            //do your logic...
            return Task.CompletedTask;
        }
    }
}

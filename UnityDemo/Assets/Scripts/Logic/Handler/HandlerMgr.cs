using Geek.Client.Message;

namespace Geek.Client
{
    public class HandlerMgr
    {
        public static void Init()
        {
            TcpMsg.Ins.MsgGetter = MsgFactory.Create;

            LoginHandler.Ins.InitHandler();
            BagHandler.Ins.InitHandler();
        }
    }
}

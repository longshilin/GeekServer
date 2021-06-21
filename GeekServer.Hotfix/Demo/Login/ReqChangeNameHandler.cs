using Geek.Server.Message.DemoLogin;
using System.Threading.Tasks;

namespace Geek.Server.Demo.Login
{
    [TcpMsgMapping(typeof(ReqChangeName))]
    public class ReqChangeNameHandler : DemoRoleActorHandler
    {
        public override async Task ActionAsync()
        {
            //这里已经在DemoRoleActor线程了
            var req = (ReqChangeName)Msg;
            if(string.IsNullOrEmpty(req.newName))
            {
                WriteAndFlush(new ResChangeName() { msg = "名字不能为空" });
                return;
            }

            if (req.newName.Length > 5)
            {
                WriteAndFlush(new ResChangeName() { msg = "名字太长了" });
                return;
            }

            //if (...) //重名检测
            //{
            //    WriteAndFlush(new ResChangeName() { msg = "名字已被占用" });
            //    return;
            //}

            var infoComp = await GetCompAgent<DemoRoleInfoCompAgent>();
            infoComp.State.RoleName = req.newName;
            WriteAndFlush(new ResChangeName() { newName = req.newName });
        }
    }
}

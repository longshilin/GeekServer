using System;
using Geek.Server.Message.DemoLogin;
using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    [TcpMsgMapping(typeof(ReqLogin))]
    public class ReqLoginHandler : BaseTcpHandler
    {
        static readonly NLog.Logger LOGGER = NLog.LogManager.GetCurrentClassLogger();
        public override async Task ActionAsync()
        {
            var agent = await ActorManager.GetOrNew<DemoLoginActorAgent>(ServerActorID.GetID(ActorType.Login));
            await agent.SendAsync(() => Login(agent, (ReqLogin)Msg));
        }

        //次函数在LoginActor内执行，也可以放到LoginActorAgent中去
        public async Task Login(DemoLoginActorAgent agent, ReqLogin reqLogin)
        {
            var res = new ResLogin();
            if (string.IsNullOrEmpty(reqLogin.userName))
            {
                res.code = 1; //账号不能为空
                WriteAndFlush(res);
                return;
            }

            if(reqLogin.platform != "android" && reqLogin.platform != "ios" && reqLogin.platform != "unity")
            {
                //验证平台合法性
                res.code = 2;
                WriteAndFlush(res);
                return;
            }

            if(reqLogin.sdkType > 0)
            {
                //TODO 通过sdktype验证sdktoken
                //可以放到玩家actor
            }

            //验证通过

            //查询角色账号，这里设定每个服务器只能有一个角色
            var playerAgent = await agent.GetCompAgent<DemoPlayerInfoCompAgent>();
            var roleId = await playerAgent.GetRoleIdOfPlayer(reqLogin.userName, reqLogin.sdkType);
            var isNewRole = roleId <= 0;
            if(isNewRole)
            {
                //没有老角色，创建新号
                roleId = IdGenerator.GetUniqueId(Settings.Ins.ServerId);
                await playerAgent.CreateRoleToPlayer(reqLogin.userName, reqLogin.sdkType, roleId);
                LOGGER.Info("创建新号:" + roleId);
            }

            //添加到chennel
            var channel = new Channel();
            channel.Id = roleId;
            channel.Time = DateTime.Now;
            channel.Ctx = Ctx;
            channel.Sign = reqLogin.device;
            ChannelManager.Add(channel);

            //登陆流程
            var roleActor = await ActorManager.GetOrNew<DemoRoleActorAgent>(roleId);
            await roleActor.OnLogin(reqLogin, isNewRole, roleId);
            var roleComp = await roleActor.GetCompAgent<DemoRoleInfoCompAgent>();
            var resLogin = await roleComp.BuildLoginMsg();
            WriteAndFlush(resLogin);

            //下行大于512会压缩
            var tipMsg = new ResNotice();
            for (int i = 0; i < 100; ++i)
                tipMsg.tip += "测试下行消息zip...";
            await Task.Delay(1000);
            WriteAndFlush(tipMsg);
        }
    }
}

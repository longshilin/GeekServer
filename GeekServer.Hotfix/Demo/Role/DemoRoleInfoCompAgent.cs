using System;
using Geek.Server.Message.DemoLogin;
using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    public class DemoRoleInfoCompAgent : StateComponentAgent<DemoRoleInfoComp, DemoRoleInfoState>
    {
        public Task OnCreate(ReqLogin reqLogin, long roleId)
        {
            State.CreateTime = DateTime.Now;
            State.Level = 1;
            State.VipLevel = 1;
            State.RoleId = roleId;
            State.RoleName = new System.Random().Next(1000, 10000).ToString();//随机给一个
            return Task.CompletedTask;
        }

        public Task OnLogin()
        {
            State.LoginTime = DateTime.Now;
            return Task.CompletedTask;
        }

        public Task OnLoginOut()
        {
            State.OfflineTime = DateTime.Now;
            return Task.CompletedTask;
        }

        public Task<ResLogin> BuildLoginMsg()
        {
            var res = new ResLogin()
            {
                code = 0,
                userInfo = new UserInfo()
                {
                    createTime = State.CreateTime.Ticks,
                    level = State.Level,
                    roleId = State.RoleId,
                    roleName = State.RoleName,
                    vipLevel = State.VipLevel
                }
            };
            return Task.FromResult(res);
        }
    }
}

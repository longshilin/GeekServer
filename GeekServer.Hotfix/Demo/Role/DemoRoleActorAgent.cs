using Geek.Server.Message.DemoLogin;
using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    public class DemoRoleActorAgent : ComponentActorAgent<DemoRoleActor>
    {
        public async Task OnLogin(ReqLogin reqLogin, bool isNewRole, long roleId)
        {
            var infoComp = await GetCompAgent<DemoRoleInfoCompAgent>();
            if (isNewRole)
            {
                await infoComp.OnCreate(reqLogin, roleId);
                var bagComp = await GetCompAgent<DemoBagCompAgent>();
                await bagComp.Init();
            }
            await infoComp.OnLogin();
        }
    }
}

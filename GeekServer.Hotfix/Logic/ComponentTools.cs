using Geek.Server.Demo;

namespace Geek.Server
{
    public class ComponentTools
    {
        public static void RegistAllComps()
        {
            //注册组件,所有actor的所有组件
            ComponentMgr.Singleton.RegistComp<ServerActor, ServerComp>();




            //sample
            ComponentMgr.Singleton.RegistComp<SampleActor, SampleComp>();

            //Demo
            ComponentMgr.Singleton.RegistComp<DemoLoginActor, DemoPlayerInfoComp>();
            ComponentMgr.Singleton.RegistComp<DemoRoleActor, DemoRoleInfoComp>();
            ComponentMgr.Singleton.RegistComp<DemoRoleActor, DemoBagComp>();
        }
    }
}
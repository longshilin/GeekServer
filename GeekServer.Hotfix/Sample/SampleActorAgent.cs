using System.Threading.Tasks;

namespace Geek.Server
{
    public class SampleActorAgent : ComponentActorAgent<SampleActor>
    {
        static readonly NLog.Logger LOGGER = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 所有的eventListener会在actor.active时自动注册
        /// 所以热更逻辑最好不要放在EventListener中（已激活的actor无法监听新的事件）
        /// addListener的类型需要是接受的泛型参数类型
        /// </summary>
        class Hotfix_EL : EventListener<ServerActorAgent, ServerActorAgent>
        {
            protected override Task HandleEvent(ServerActorAgent agent, Event evt)
            {
                LOGGER.Info("测试热更完成");
                //配置表
                var bean = Config.ConfigBean.GetBean<Config.t_languageBean, int>(2);
                LOGGER.Info("打印语言表配置表内容：" + bean.t_content);
                var tb = Config.ConfigBean.GetBean<Config.t_test2Bean, int>(1);
                LOGGER.Info("打印t_test2Bean配置表内容：" + tb.t_str2);
                return Task.CompletedTask;
            }

            protected override Task InitListener(ServerActorAgent actor)
            {
                actor.Actor.EvtDispatcher.AddListener(EventID.HotfixEnd, this);
                return Task.CompletedTask;
            }
        }


        /// <summary>
        /// timerHander的泛型参数必须和添加时的this一致，debug模式下有检测
        /// </summary>
        class Test_TH : TimerHandler<SampleActorAgent>
        {
            protected override Task HandleTimer(SampleActorAgent agent, Param param)
            {
                LOGGER.Info(((OneParam<string>)param).value);
                return agent.callWhen14_00();
            }
        }

        public override async Task Active()
        {
            //actor激活后回调，用于初始化
            await base.Active();

            //添加一个每天下午2点的回调
            var id = this.AddDailySchedule<Test_TH>(14, 00, new OneParam<string>("test timer param"));

            //TimerId不能存在agent,只能存在Actor上（存在agent上，热更后就没有值了，因为换了dll）
            Actor.TimerId = id;


            var comp = await GetCompAgent<SampleCompAgent>();
            await comp.ChangeStateContent();
        }

        Task callWhen14_00()
        {
            LOGGER.Info("14点了, 该上班了");
            return Task.CompletedTask;
        }

        public override Task Deactive()
        {
            //actor回收前回调，用于销毁处理，比如移除timer
            //Deactive中不能修改State
            this.Unschedule(Actor.TimerId);
            return base.Deactive();
        }
    }
}

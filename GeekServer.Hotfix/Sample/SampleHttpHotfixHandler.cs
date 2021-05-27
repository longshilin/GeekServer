using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Geek.Server.Test
{
    /// <summary>
    /// 非debug模式(server_config.json中修改)需要验证token
    /// </summary>
    [HttpMsgMapping("hotfix")]
    public class SampleHttpHotfixHandler : BaseHttpHandler
    {
        public override bool Inner => true;

        public override async Task<string> Action(string ip, string url, Dictionary<string, string> paramMap)
        {
            await HotfixMgr.ReloadModule("");
            //这里只是简单的测试，你应该需要更多的操作，比如验证dll目录，dll是否存在，dll的合法性等
            var server = await ActorManager.GetOrNew<ServerActorAgent>(ServerActorID.GetID(ActorType.Normal));
            server.Actor.EvtDispatcher.DispatchEvent(EventID.HotfixEnd);
            return HttpResult.Success;
        }
    }

    //http://localhost:20000/geek/server/logic?cmd=get_hotfix_url
    [HttpMsgMapping("get_hotfix_url")]
    public class SampleGetHotfixSignHandler: BaseHttpHandler
    {
        public override bool Inner => false;
        public override bool CheckSign => false;

        public override Task<string> Action(string ip, string url, Dictionary<string, string> paramMap)
        {
            var time = DateTime.UtcNow.Ticks;
            var str = Settings.Ins.HttpInnerCode + time;
            var sign = GetStringSign(str, true);
            return Task.FromResult($"http://localhost:20000/geek/server/logic?cmd=hotfix&time={time}&sign={sign}");
        }
    }
}

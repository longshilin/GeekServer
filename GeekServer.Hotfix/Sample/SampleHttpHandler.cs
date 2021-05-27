using System.Collections.Generic;
using System.Threading.Tasks;

namespace Geek.Server
{
    /// <summary>
    /// 通过标签自动解析，热更后自动生效
    /// http://localhost:20000/geek/server/logic?cmd=test
    /// </summary>
    [HttpMsgMapping("test")]
    public class SampleHttpHandler : BaseHttpHandler
    {
        public override bool Inner => false;
        public override bool CheckSign => false;

        public override Task<string> Action(string ip, string url, Dictionary<string, string> paramMap)
        {
            return Task.FromResult((string)HttpResult.Success);
        }
    }
}

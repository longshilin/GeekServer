using System.Collections.Generic;

namespace Geek.Server.Demo
{
    public class DemoPlayerInfoComp : QueryComponent
    {
        //仅在内存中缓存
        public Dictionary<string, DemoPlayerInfoState> PlayerMap = new Dictionary<string, DemoPlayerInfoState>();
    }
}

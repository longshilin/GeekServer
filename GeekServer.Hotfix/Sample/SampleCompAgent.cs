using System.Threading.Tasks;

namespace Geek.Server
{
    public class SampleCompAgent : StateComponentAgent<SampleComp, SampleState>
    {
        public override Task Active()
        {
            //component激活后回调，用于初始化
            return base.Active();
        }

        public Task ChangeStateContent()
        {
            State.TestNoStoreList.Add(100);
            State.TestLong = 1234L;
            State.TestMap[1] = "geek";
            State.TestMap[2] = "server";
            State.TestList.Add("geek");
            State.TestList.Add("server");
            State.TestStr = "geek.server." + System.DateTime.Now;
            //正常关服然后查看数据库中的数据
            return Task.CompletedTask;
        }

        public override Task Deactive()
        {
            //component回收前回调，用于销毁处理，比如移除timer
            //Deactive中不能修改State
            return base.Deactive();
        }
    }
}

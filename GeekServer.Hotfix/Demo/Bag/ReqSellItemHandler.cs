using Geek.Server.Message.DemoBag;
using Geek.Server.Message.DemoLogin;
using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    [TcpMsgMapping(typeof(ReqSellItem))]
    public class ReqSellItemHandler : DemoRoleActorHandler
    {
        public override async Task ActionAsync()
        {
            var req = (ReqSellItem)Msg;
            var bagComp = await GetCompAgent<DemoBagCompAgent>();
            if(!bagComp.State.ItemMap.ContainsKey(req.itemId))
            {
                await notice("玩家没有出售的道具");
                return;
            }

            if (bagComp.State.ItemMap[req.itemId] <= 0)
            {
                await notice("玩家没有出售的道具");
                return;
            }

            var bean = Config.ConfigBean.GetBean<Config.t_itemBean, int>(req.itemId);
            if(bean == null)
            {
                await notice("出售的道具找不到配置");
                return;
            }

            if(bean.t_can_sell == 0)
            {
                await notice("道具不能出售");
                return;
            }

            var res = new ResItemChange();
            await bagComp.CutItem(req.itemId, 1);//一次出售一个
            res.itemDic.Add(req.itemId, -1);

            var param = bean.t_sell_num.SplitTo2IntArray(';', '+');
            foreach(var arr in param)
            {
                if (arr.Length < 2)
                    continue;
                await bagComp.AddItem(arr[0], arr[1]);
                if (res.itemDic.ContainsKey(arr[0]))
                    res.itemDic[arr[0]] += arr[1];
                else
                    res.itemDic.Add(arr[0], arr[1]);
            }
            WriteAndFlush(res);
        }

        Task notice(string msg)
        {
            var res = new ResNotice();
            res.tip = msg;
            WriteAndFlush(res);
            return Task.CompletedTask;
        }
    }
}

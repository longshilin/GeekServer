using Geek.Server.Message.DemoBag;
using Geek.Server.Message.DemoLogin;
using System.Threading.Tasks;

namespace Geek.Server.Demo
{
    [TcpMsgMapping(typeof(ReqUseItem))]
    public class ReqUseItemHandler : DemoRoleActorHandler
    {
        public override async Task ActionAsync()
        {
            var req = (ReqUseItem)Msg;
            var bagComp = await GetCompAgent<DemoBagCompAgent>();

            if (!bagComp.State.ItemMap.ContainsKey(req.itemId))
            {
                await notice("玩家没有使用的道具");
                return;
            }

            if (bagComp.State.ItemMap[req.itemId] <= 0)
            {
                await notice("玩家没有使用的道具");
                return;
            }

            var bean = Config.ConfigBean.GetBean<Config.t_itemBean, int>(req.itemId);
            if (bean == null)
            {
                await notice("使用的道具找不到配置");
                return;
            }

            if (bean.t_use_type == 0)
            {
                await notice("道具不能使用");
                return;
            }

            if(bean.t_use_type != 1 && bean.t_use_type != 2)
            {
                await notice("道具使用类型未实现");
                return;
            }

            var res = new ResItemChange();
            await bagComp.CutItem(req.itemId, 1);//一次出售一个
            res.itemDic.Add(req.itemId, -1);

            switch(bean.t_use_type)
            {
                case 1: //宝箱
                    {
                        var param = bean.t_param.SplitTo2IntArray(';', '+');
                        foreach (var arr in param)
                        {
                            if (arr.Length < 2)
                                continue;
                            await bagComp.AddItem(arr[0], arr[1]);
                            if (res.itemDic.ContainsKey(arr[0]))
                                res.itemDic[arr[0]] += arr[1];
                            else
                                res.itemDic.Add(arr[0], arr[1]);
                        }
                    }
                    break;
                case 2: //经验丹
                    {
                        int.TryParse(bean.t_param, out var level);
                        var infoComp = await GetCompAgent<DemoRoleInfoCompAgent>();
                        infoComp.State.Level += level;
                        var levelMsg = new ResLevelUp();
                        levelMsg.level = infoComp.State.Level;
                        WriteAndFlush(levelMsg);
                    }
                    break;
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

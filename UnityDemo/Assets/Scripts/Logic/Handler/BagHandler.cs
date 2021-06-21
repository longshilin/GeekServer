using Geek.Client.Message.DemoBag;
using System;
using System.Collections.Generic;

namespace Geek.Client
{
    public class BagHandler : MsgHandler<BagHandler>
    {
        public Action OnBagChange;
        public Dictionary<int, long> ItemMap = new Dictionary<int, long>();
        public override void InitHandler()
        {
            TcpMsg.Ins.RegistEventHandler(ResBagInfo.MsgId, onResBagInfo);
            TcpMsg.Ins.RegistEventHandler(ResItemChange.MsgId, onResItemChange);
        }

        void onResBagInfo(BaseMessage msg)
        {
            var res = (ResBagInfo)msg;
            ItemMap = res.itemDic;
            OnBagChange?.Invoke();
        }

        void onResItemChange(BaseMessage msg)
        {
            var res = (ResItemChange)msg;
            var zeroList = new List<int>();
            foreach (var kv in res.itemDic)
            {
                if(ItemMap.ContainsKey(kv.Key))
                    ItemMap[kv.Key] += kv.Value;
                else
                    ItemMap[kv.Key] = kv.Value;

                if (ItemMap[kv.Key] <= 0)
                    zeroList.Add(kv.Key);
            }
            foreach (var id in zeroList)
            {
                if (ItemMap.ContainsKey(id))
                    ItemMap.Remove(id);
            }
            OnBagChange?.Invoke();
        }
    }
}

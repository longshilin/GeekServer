using Geek.Client.Message.DemoBag;
using UnityEngine;
using UnityEngine.UI;

namespace Geek.Client
{
    public class ItemInfoWindow : MonoBehaviour
    {
        public Button CloseBtn;
        public Text NameTxt;
        public Text DescTxt;
        public Button SellBtn;
        public Button UseBtn;

        int cacheId;
        void Awake()
        {
            CloseBtn.onClick.AddListener(() => { Destroy(gameObject); });
            SellBtn.onClick.AddListener(sellItem);
            UseBtn.onClick.AddListener(useItem);
        }

        void sellItem()
        {
            var req = new ReqSellItem();
            req.itemId = cacheId;
            TcpMsg.Ins.SendMsg(req);
            Destroy(gameObject);
        }

        void useItem()
        {
            var req = new ReqUseItem();
            req.itemId = cacheId;
            TcpMsg.Ins.SendMsg(req);
            Destroy(gameObject);
        }

        public void BindItem(int id, long num)
        {
            cacheId = id;
            var bean = Config.ConfigBean.GetBean<Config.t_itemBean, int>(id);
            if(bean != null)
            {
                NameTxt.text = bean.t_name;
                DescTxt.text = $"拥有：{num}\n" + bean.t_desc;
                SellBtn.interactable = bean.t_can_sell == 1;
                UseBtn.interactable = bean.t_use_type != 0;
            }
        }
    }
}

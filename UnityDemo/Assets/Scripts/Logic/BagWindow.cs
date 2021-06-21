using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Geek.Client
{
    public class BagWindow : MonoBehaviour
    {
        public Text GoldTxt;
        public Button CloseBtn;
        public Transform ItemTemplate;
        public Transform ItemContainer;

        Dictionary<int, Transform> cacheMap = new Dictionary<int, Transform>();
        void Awake()
        {
            CloseBtn.onClick.AddListener(() => Destroy(gameObject));
            BagHandler.Ins.OnBagChange += onBagChange;
            onBagChange();
        }

        void OnDestroy()
        {
            BagHandler.Ins.OnBagChange -= onBagChange;
        }

        void onBagChange()
        {
            foreach (var kv in cacheMap)
                Destroy(kv.Value.gameObject);
            cacheMap.Clear();

            var map = BagHandler.Ins.ItemMap;
            foreach(var kv in map)
            {
                var bean = Config.ConfigBean.GetBean<Config.t_itemBean, int>(kv.Key);
                if (bean != null && bean.t_show == 0)
                    continue;

                var item = Instantiate(ItemTemplate);
                item.gameObject.SetActive(true);
                var name = item.Find("TextName").GetComponent<Text>();
                if(bean != null)
                    name.text = bean.t_name;
                else
                    name.text = kv.Key + "";

                var num = item.Find("TextNum").GetComponent<Text>();
                num.text = kv.Value + "";

                var copyKv = kv;
                var btn = item.GetComponent<Button>();
                btn.onClick.AddListener(()=> {
                    var infoPrefab = Resources.Load<GameObject>("ItemInfoPanel");
                    var clone = Instantiate(infoPrefab);
                    clone.transform.SetParent(this.transform.parent);
                    clone.transform.localPosition = Vector3.zero;
                    var info = clone.GetComponent<ItemInfoWindow>();
                    info.BindItem(copyKv.Key, copyKv.Value);
                });

                cacheMap.Add(kv.Key, item);
                item.SetParent(ItemContainer);
            }

            var gold = 0L;
            if (map.ContainsKey(103))
                gold = map[103];
            GoldTxt.text = "金币：" + gold;
        }
    }
}
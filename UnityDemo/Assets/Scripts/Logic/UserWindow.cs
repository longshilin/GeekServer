using Geek.Client.Message.DemoBag;
using Geek.Client.Message.DemoLogin;
using UnityEngine;
using UnityEngine.UI;

namespace Geek.Client
{
    public class UserWindow : MonoBehaviour
    {
        public Text InfoText;
        public InputField NewNameTxt;
        public Button ChangeNameBtn;
        public Button BagBtn;

        void Awake()
        {
            onRoleInfoChange();
            LoginHandler.Ins.OnUserInfoChange += onRoleInfoChange;
            ChangeNameBtn.onClick.AddListener(onChangeName);
            BagBtn.onClick.AddListener(openBag);
        }

        void onRoleInfoChange()
        {
            var info = LoginHandler.Ins.UserData;
            if (info == null)
                return;
            InfoText.text = $"名字:{info.roleName}   等级:{info.level}   vip等级:{info.vipLevel}";
        }

        void onChangeName()
        {
            if(string.IsNullOrEmpty(NewNameTxt.text))
            {
                TipView.Ins.Notice("新名字为空");
                return;
            }

            var req = new ReqChangeName();
            req.newName = NewNameTxt.text;
            TcpMsg.Ins.SendMsg(req);
        }

        void openBag()
        {
            var req = new ReqBagInfo();
            TcpMsg.Ins.SendMsg(req);

            var prefab = Resources.Load<GameObject>("BagPanel");
            var ins = Instantiate(prefab);
            ins.transform.SetParent(InfoText.canvas.transform, false);
            ins.transform.localPosition = Vector3.zero;
        }

        void OnDestroy()
        {
            LoginHandler.Ins.OnUserInfoChange += onRoleInfoChange;
        }
    }
}
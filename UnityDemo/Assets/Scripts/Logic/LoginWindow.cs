using Geek.Client.Message.DemoLogin;
using UnityEngine;
using UnityEngine.UI;

namespace Geek.Client
{
    public class LoginWindow : MonoBehaviour
    {
        public InputField ipTxt;
        public InputField nameTxt;
        public Button loginBtn;

        void Start()
        {
            HandlerMgr.Init();
            loginBtn.onClick.AddListener(onLoin);
        }

        void onLoin()
        {
            if (string.IsNullOrEmpty(ipTxt.text))
            {
                TipView.Ins.Notice("请输入ip和端口");
                return;
            }

            if (string.IsNullOrEmpty(nameTxt.text.Trim()))
            {
                TipView.Ins.Notice("用户名为空");
                return;
            }

            var url = ipTxt.text;
            var arr = url.Split(':');
            if(url.Length < 2)
            {
                TipView.Ins.Notice("请输入正确的ip和端口");
                return;
            }

            int.TryParse(arr[1], out var port);
            if(port <= 0)
            {
                TipView.Ins.Notice("请输入正确的ip和端口");
                return;
            }
            var user = nameTxt.text;
            Debug.Log($"连接服务器：{url}");
            if (TcpMsg.Ins.Connect(arr[0], port))
            {
                //登陆
                var req = new ReqLogin();
                req.sdkType = 0;
                req.sdkToken = "";
                req.userName = user;
                req.device = SystemInfo.deviceUniqueIdentifier;
                if (Application.platform == RuntimePlatform.Android)
                    req.platform = "android";
                else if (Application.platform == RuntimePlatform.IPhonePlayer)
                    req.platform = "ios";
                else
                    req.platform = "unity";

                TcpMsg.Ins.SendMsg(req);
            }
            else
            {
                TipView.Ins.Notice("连接服务器失败");
            }
        }
    }
}
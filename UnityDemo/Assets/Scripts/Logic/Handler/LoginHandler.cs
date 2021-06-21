using Geek.Client.Message.DemoLogin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Geek.Client
{
    public class LoginHandler : MsgHandler<LoginHandler>
    {
        public Action OnUserInfoChange;
        public UserInfo UserData { get; set; }
        public override void InitHandler()
        {
            //登陆系统服务器下行消息监听
            TcpMsg.Ins.RegistEventHandler(ResLogin.MsgId, onResLogin);
            TcpMsg.Ins.RegistEventHandler(ResLevelUp.MsgId, onResLevelUp);
            TcpMsg.Ins.RegistEventHandler(ResNotice.MsgId, onResNotice);
            TcpMsg.Ins.RegistEventHandler(ResChangeName.MsgId, onResChangeName);
        }

        void onResLogin(BaseMessage msg)
        {
            var res = (ResLogin)msg;
            if(res.code != 0)
            {
                Debug.LogError("登陆失败，code=" + res.code);
                return;
            }
            Debug.Log("登陆成功");
            UserData = res.userInfo;
            OnUserInfoChange?.Invoke();
            SceneManager.LoadScene("Main");
        }

        void onResLevelUp(BaseMessage msg)
        {
            var res = (ResLevelUp)msg;
            UserData.level = res.level;
            OnUserInfoChange?.Invoke();
            Debug.Log("等级变化 level=" + res.level);
            TipView.Ins.Notice($"等级提升，当前{res.level}级");
        }

        void onResNotice(BaseMessage msg)
        {
            var res = (ResNotice)msg;
            TipView.Ins.Notice(res.tip);
            Debug.Log("服务器通知" + res.tip);
        }

        void onResChangeName(BaseMessage msg)
        {
            var res = (ResChangeName)msg;
            if(!string.IsNullOrEmpty(res.msg))
            {
                TipView.Ins.Notice(res.msg);
                return;
            }
            UserData.roleName = res.newName;
            OnUserInfoChange?.Invoke();
        }
    }
}
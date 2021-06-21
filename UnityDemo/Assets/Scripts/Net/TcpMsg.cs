using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Geek.Client
{
    public class TcpMsg : MonoBehaviour
    {
        public static TcpMsg Ins { get; private set; }

        void Awake()
        {
            GameObject.DontDestroyOnLoad(gameObject);
            Ins = this;

        }

        void OnDestroy()
        {
            tcp.Close();
        }

        TcpSocket tcp = new TcpSocket();
        public bool Connect(string ip, int port)
        {
            return tcp.Connect(ip, port);
        }


        Dictionary<int, Action<BaseMessage>> evtMap = new Dictionary<int, Action<BaseMessage>>();
        public Func<int, BaseMessage> MsgGetter { get; set; }
        public void RegistEventHandler(int msgId, Action<BaseMessage> handAction)
        {
            evtMap[msgId] = handAction;
        }

        int msgOder = 1;
        public void SendMsg(BaseMessage msg)
        {
            //服务器解码参考https://github.com/leeveel/GeekServer/blob/master/GeekServer.Core/Net/Tcp/TcpServerDecoder.cs
            var headerLen = 4 + 8 + 4 + 4;//int(消息大小)+long(时间)+int(checkNumber)+int(msgId)
            var data = new byte[512];
            var len = msg.Write(data, headerLen);//空出消息头的位置
            if(len > data.Length)//msg第1次wirte如果buffer不够的话需要从新装一次数据
            {
                data = new byte[len];
                msg.Write(data, headerLen);
            }

            //编码，于服务器解码对应
            int msgLen = len;
            var time = System.DateTime.Now.Ticks / 10000;//当前毫秒数
            var checkNum = msgOder++;
            checkNum ^= (0x1234 << 8);//0x1234加密参数，需要和服务器解码是对应,用于验证消息合法性
            checkNum ^= msgLen;

            var msgId = msg.GetMsgId();
            int offset = 0;
            XBuffer.WriteInt(msgLen, data, ref offset);
            XBuffer.WriteLong(time, data, ref offset);
            XBuffer.WriteInt(checkNum, data, ref offset);
            XBuffer.WriteInt(msgId, data, ref offset);

            tcp.Send(data, len);
        }

        byte[] readBuffer = new byte[512];
        byte[] remainBuffer = new byte[512]; //缓存分包/黏包;
        byte[] cacheBuffer = new byte[512];
        int remainSize = 0;

        public void Update()
        {
            var len = tcp.ReadData(readBuffer);
            //服务器编码参考https://github.com/leeveel/GeekServer/tree/master/GeekServer.Core/Net/Tcp/TcpServerEncoder.cs
            if(len > 0)
            {
                var dataSize = remainSize + len;
                if (dataSize > remainBuffer.Length)//扩容
                {
                    var tmp = new byte[dataSize];
                    Array.Copy(remainBuffer, 0, tmp, 0, remainSize);
                    remainBuffer = tmp;
                }
                Array.Copy(readBuffer, 0, remainBuffer, remainSize, len);
                remainSize = dataSize;
            }

            //消息头长度都不够
            if (remainSize < sizeof(int))
                return;

            //解码需要服务器编码一致才能解析出来
            int offset = 0;
            var packageSize = XBuffer.ReadInt(remainBuffer, ref offset);//当前消息包大小
            bool isZip = packageSize < 0;
            if (packageSize < 0)
                packageSize = -packageSize;
            if (remainSize < packageSize)//读取的长度不够
                return;

            var msgId = XBuffer.ReadInt(remainBuffer, ref offset);//消息id
            var msg = MsgGetter(msgId);
            if(isZip)
            {
                var data = unZip(msgId, remainBuffer, offset, packageSize - offset);
                msg.Read(data, 0);
            }
            else
            {
                msg.Read(remainBuffer, offset);
            }

            //保存剩余的buffer
            remainSize -= packageSize;
            if(remainSize > 0)
            {
                if(remainSize > cacheBuffer.Length)
                    cacheBuffer = new byte[remainSize];
                //remainBuffer中未使用的数据读出来,移动0的位置
                Array.Copy(remainBuffer, packageSize, cacheBuffer, 0, remainSize);
                Array.Copy(cacheBuffer, 0, remainBuffer, 0, remainSize);
            }

            //分发消息到logic
            if (evtMap.ContainsKey(msgId))
                evtMap[msgId](msg);
            else
                Debug.LogWarning("未监听的网络消息：" + msgId);
        }

        byte[] unZip(int msgId, byte[] before, int offset, int zipSize)
        {
            try
            {
                if (before == null)
                    return null;
                using (MemoryStream ms = new MemoryStream(before, offset, zipSize))
                {
                    using (ZipInputStream zipStream = new ZipInputStream(ms))
                    {
                        zipStream.IsStreamOwner = true;
                        var file = zipStream.GetNextEntry();
                        var after = new byte[(int)file.Size];
                        zipStream.Read(after, 0, (int)file.Size);
                        return after;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"消息解压失败>{msgId}\n{e.ToString()}");
            }
            return null;
        }
    }
}


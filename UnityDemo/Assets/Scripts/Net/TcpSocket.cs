using System;
using System.Net.Sockets;
using UnityEngine;

//仅用于Demo，没做任何优化处理
namespace Geek.Client
{
    public class TcpSocket
    {
        TcpClient client;
        public bool IsConnected => client != null && client.Connected;
        public bool Connect(string ip, int port)
        {
            AddressFamily ipType = AddressFamily.InterNetwork;
            client = new TcpClient(ipType);

            try
            {
                client.Connect(ip, port);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return false;
            }
            return true;
        }

        public void Close()
        {
            if (client != null && client.Connected)
                client.Close();
            client = null;
        }

        public void Send(byte[] data, int len)
        {
            if (!IsConnected)
                return;
            try
            {
                client.GetStream().Write(data, 0, len);
                client.GetStream().Flush();
            }
            catch(Exception e)
            {
                //可能已经断开连接了
                Debug.LogError(e.ToString());
            }
        }

        public int ReadData(byte[] buffer)
        {
            if (!IsConnected)
                return 0;
            var stream = client.GetStream();
            if (!stream.DataAvailable)
                return 0;
            int len = stream.Read(buffer, 0, buffer.Length);
            return len;
        }
    }
}


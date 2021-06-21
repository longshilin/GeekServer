namespace Geek.Client
{
    public class BaseMessage
    {
        public virtual int Read(byte[] buffer, int offset)
        {
            return offset;
        }

        public virtual int Write(byte[] buffer, int offset)
        {
            return offset;
        }

        public virtual int GetMsgId()
        {
            return 0;
        }

        public byte[] GetData()
        {
            var data = new byte[512];
            int offset = this.Write(data, 0);
            if (offset > data.Length)
            {
                data = new byte[offset];
                offset = this.Write(data, 0);
            }
            return data;
        }
    }
}
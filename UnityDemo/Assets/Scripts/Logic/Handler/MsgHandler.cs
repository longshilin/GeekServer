
namespace Geek.Client
{
    public abstract class MsgHandler<T> where T : new()
    {
        public static readonly T Ins = new T();
        public abstract void InitHandler();
    }
}
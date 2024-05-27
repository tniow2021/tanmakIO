using sex.Pooling;
using System.Net.Sockets;

namespace sex
{
    public class Root
    {
        public static Root root { get; private set; }
        public PoolEngine poolEngine { get; private set; }
        public IPool<byte[]> byte10000arrayPool { get; private set; }
        public IPool<SocketAsyncEventArgs> socketArgsPool { get; private set; }
        public Root()
        {
            root = this;
            poolEngine=new PoolEngine();

            //아이디와 스레드 안정성... 어떨게해야좋을 것인가.
            Func<byte[]> fb = () => { return new byte[10000]; };
            byte10000arrayPool = poolEngine.CreatePool(fb,n:100, id:1);
        }
        
    }
}

using sex.Conversion;
using sex.DataStructure;
using sex.Networking;
using sex.Pooling;
namespace sex
{
    public class Root
    {
        public static Root root { get; private set; }
        public PoolEngine poolEngine { get; private set; }
        public IPool<byte[]> byte10000arrayPool { get; private set; }
        public IPool<byte[]> byte2000arrayPool { get; private set; }
        public IPool<UserIO> UserIOPool { get; private set; }



        public IntTable NetPacketMinimumLengthTable { get; private set; }



        public Root()
        {
            root = this;
            poolEngine = new PoolEngine();
            NetPacketMinimumLengthTable = new IntTable(highestNumber: 1000);

            //아이디와 스레드 안정성... 어떨게해야좋을 것인가.
            Func<byte[]> fb = () => { return new byte[10000]; };
            byte10000arrayPool = poolEngine.CreateBasicTypePool(fb, n: 100,ThreadSafe:true);
            Func<byte[]> fb2 = () => { return new byte[1000]; };
            byte2000arrayPool = poolEngine.CreateBasicTypePool(fb2, n: 100, ThreadSafe: true);

            Func<UserIO> ui = () => { return new UserIO(packetSizeLimit: 1024); };
            UserIOPool = poolEngine.CreatePool<UserIO>(ui, n: 100);

        }
    }
}

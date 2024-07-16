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
        public IPool<UserIO> UserIOPool { get; private set; }
        //public Table<IPool<IPool<INetConvertible>>> table;



        public Table<int> NetPacketMinimumLengthTable { get; private set; }



        public Root()
        {
            root = this;
            poolEngine = new PoolEngine();

            //아이디와 스레드 안정성... 어떨게해야좋을 것인가.
            Func<byte[]> fb = () => { return new byte[10000]; };
            byte10000arrayPool = poolEngine.CreateBasicTypePool(fb, n: 100);

            Func<UserIO> ui = () => { return new UserIO(packetSizeLimit: 1024); };
            UserIOPool = poolEngine.CreatePool<UserIO>(ui, n: 100);

            //NetConvertibleSetting();
        }
        //static void NetConvertibleSetting()
        //{
        //    root.table = new Table<IPool<IPool<INetConvertible>>>(highestNumber: 0);
        //    var table = root.table;

        //    Vecter3Int.SetTypeNumber(0);
        //    NetConvertibleTabling<Vecter3Int>(
        //        table, () => { return new Vecter3Int(); }, typeNumber: 0, pullingSize: 100, nPool: 10);


        //}
        //static void NetConvertibleTabling<T>(Table<IPool<IPool<INetConvertible>>> table,
        //    Func<T> constructor, short typeNumber, int pullingSize, int nPool) where T : INetConvertible
        //{
        //    IPool<IPool<INetConvertible>> pool1 = Root.root.poolEngine.CreateBasicTypePool<IPool<INetConvertible>>
        //        (
        //            constructor: () =>
        //            {
        //                return Root.root.poolEngine.CreateBasicTypePool<INetConvertible>(() => { return constructor(); }, pullingSize);
        //            },
        //            n: nPool);
        //    table.Register(pool1, numbering: typeNumber);
        //}
    }
}

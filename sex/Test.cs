using sex.Pooling;
using System.Net;
using System.Net.Sockets;
using sex.GameLogic;
using sex.NetPackets;

namespace sex
{
    public static class Test
    {
        public static void AllTest()
        {
            //PoolTest();
            //UserIOTest();
            //INetStructTest();
            //DecoderTest();
            DividerTest();

        }
        static void DividerTest()
        {
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sk.Connect(new IPEndPoint(IPAddress.Loopback, 2024));
            Console.WriteLine("연결함");
            User user = new User(sk);
            var v3 = new Vector3Int();
            v3.x = 4;
            v3.y = 5;
            v3.z = 6;

            Thread thread = new Thread(() => {
                for (int i = 0; i < 1000; i++)
                {
                    v3.z += 1;
                    user.Send(ref v3);
                    //Console.WriteLine("전송함");
                }
            });
            thread.Start();
            
        }
        //static void INetStructTest()
        //{
        //    Vecter3Int s= new Vecter3Int(7,8,9);
        //    Span<byte> span = new byte[2000];
        //    int i = 0;
        //    s.Encode(span, ref i);
        //    s.x = 3;
        //    i = 0;
        //    s.Decode(span,ref i);
        //    Console.WriteLine(s.x+" "+s.y+" "+s.z);
        //}
        //static unsafe void DecoderTest()
        //{
        //    var table = Root.root.table;

        //    int count = 0;
        //    ConvertibleGroup group = new ConvertibleGroup(table);
        //    EnDecoder enDecoder = new EnDecoder(group,
        //        (INetConvertible data) =>{
        //            Vecter3Int mydata = (Vecter3Int)data;
        //            count++;
        //            group.ReturnBlock(data);
        //        }
        //    );


        //    Stopwatch sw = new Stopwatch();

        //    DynamicBuff<byte> myBuff = new DynamicBuff<byte>(new byte[1000000]);
        //    Vecter3Int myData = new Vecter3Int(0, 0, 0);
        //    sw.Start();
        //    for (int c = 0; c < 100; c++)
        //    {
        //        for (int i = 0; i < 100000; i++)
        //        {
        //            myData.y = i;
        //            if(enDecoder.Encode(myBuff, myData))
        //            {

        //            }
        //            else
        //            {
        //                break;
        //            }
        //            //if(myBuff.Write(3,out Span<byte>span))
        //            //{
        //            //    span[0] = 0b11111111;
        //            //    span[1] = 0b11111111;
        //            //    span[2] = 0b11111111;
        //            //}
        //        }
        //        //Console.WriteLine("ssssssssssssss");
        //        int nByteProcessed = 0;
        //        if (myBuff.NonCountingRead(out Span<byte> span))
        //        {
        //            nByteProcessed = enDecoder.Decode(span);
        //            myBuff.IncreaseReadOffset(nByteProcessed);
        //        }
        //    }
        //    sw.Stop();
        //    Console.WriteLine("rgrgL " + count + " time:" + sw.ElapsedMilliseconds);
        //}
        public static void PoolTest()
        {
            PoolEngine pm = Root.root.poolEngine;
            pm.PrintPoolStatistics();
            Func<abc> aa = () => { return new abc(); };
            for (int i = 0; i < 30; i++)
            {
                IPool<abc> abcPool = Root.root.poolEngine.CreateBasicTypePool<abc>(aa, 4000,ThreadSafe:false);
                abc a;
                for (int j = 0; j < 800; j++)
                {
                    a = abcPool.GetBlock();
                }
                for (int j = 0; j < 1000; j++)
                {
                    abcPool.RepayBlock(new abc());
                }
            }
            pm.PrintPoolStatistics();
        }
        //public static void UserIOTest()
        //{
        //    Socket listener = new Socket(
        //        AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //    listener.Bind(new IPEndPoint(IPAddress.Any, 20240));
        //    listener.Listen(10);
        //    Socket client = listener.Accept();
        //    Console.WriteLine("누군가 왔다.");
        //    UserIO a = Root.root.UserIOPool.GetBlock();

        //    a.SetUserIO(client,packetSizeLimit: 1024);
        //    a.recieveEvent = (Span<byte> span) =>
        //    {
        //        int count = 0;
        //        if (span.Length > 5)
        //        {
        //            for (int i = 0; i < span.Length / 5; i++)
        //            {
        //                for (int j = 0; j < 5; j++)
        //                {
        //                    Console.Write(span[i * 5 + j]);
        //                    count++;
        //                }
        //                Console.Write("\n");
        //            }
        //            return count;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    };
        //    a.errorEvent = (UserIOError u) => { Console.WriteLine(u.ToString()); Root.root.UserIOPool.RepayBlock(a); };
        //    a.ReciveStart();
        //}
        public class abc
        {
            int a, b, c;
        }
    }
}

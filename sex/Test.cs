using sex.Conversion;
using sex.DataStructure;
using sex.Networking;
using sex.Pooling;
using sex.UserDefinedNetPacket;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace sex
{
    public static class Test
    {
        public static void AllTest()
        {
            //PoolTest();
            //UserIOTest();
            INetStructTest();
            DecoderTest();
        }
        static void INetStructTest()
        {
            Vecter3Int s= new Vecter3Int(7,8,9);
            Span<byte> span = new byte[2000];
            int i = 0;
            s.Encode(span, ref i);
            s.x = 3;
            i = 0;
            s.Decode(span,ref i);
            Console.WriteLine(s.x+" "+s.y+" "+s.z);
        }
        static void DecoderTest()
        {
            Table<IPool<IPool<INetConvertible>>> table = new Table<IPool<IPool<INetConvertible>>>(1);
            NetStructTableSetting(table);

            ConvertibleGroup group = new ConvertibleGroup(table);
            EnDecoder enDecoder = new EnDecoder((INetConvertible data) =>
            {
                Vecter3Int mydata = (Vecter3Int)data;
                
                Console.WriteLine("됨"+mydata.x+" "+mydata.y+" "+mydata.z);
            }, group);

            

            DynamicBuff<byte> myBuff = new DynamicBuff<byte>(new byte[10000]);
            for(int i=0;i<280;i++)
            {
                Vecter3Int myData = new Vecter3Int(542551342,-i,81414145);
                enDecoder.Encode(myBuff, myData);
            }

            int nByteProcessed = 0;
            do
            {
                if (myBuff.NonCountingRead(out Span<byte> span))
                {
                    nByteProcessed = enDecoder.Decode(span);
                    myBuff.IncreaseReadOffset(nByteProcessed);
                }
            }
            while (nByteProcessed == 0);
            
        }
        static void NetStructTableSetting(Table<IPool<IPool<INetConvertible>>> table)
        {
            IPool<IPool<INetConvertible>> pool1 = Root.root.poolEngine.CreateBasicTypePool<IPool<INetConvertible>>(
                () =>
                {
                    return Root.root.poolEngine.CreateBasicTypePool<INetConvertible>(() => { return new Vecter3Int(); }, 20);
                }, n: 10);
            Vecter3Int.typeNumber = 0;
            table.Register(pool1, numbering: 0);

            IPool<IPool<INetConvertible>> pool2 = Root.root.poolEngine.CreateBasicTypePool<IPool<INetConvertible>>(
            () =>
            {
                return Root.root.poolEngine.CreateBasicTypePool<INetConvertible>(() => { return new TestClass(); }, 20);
            }, n: 10);
            TestClass.typeNumber = 1;
            table.Register(pool2, numbering: 1);


        }
        public static void PoolTest()
        {
            PoolEngine pm = Root.root.poolEngine;
            pm.PrintPoolStatistics();
            Func<abc> aa = () => { return new abc(); };
            for (int i = 0; i < 30; i++)
            {
                IPool<abc> abcPool = Root.root.poolEngine.CreateBasicTypePool<abc>(aa, 4000);
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
        public static void UserIOTest()
        {
            Socket listener = new Socket(
                AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 20240));
            listener.Listen(10);
            Socket client = listener.Accept();
            Console.WriteLine("누군가 왔다.");
            UserIO a = Root.root.UserIOPool.GetBlock();

            a.SetUserIO(client, 9, packetSizeLimit: 1024, (Span<byte> span) =>
            {
                int count=0;
                if(span.Length > 5)
                {
                    for(int i=0;i<span.Length/5;i++)
                    {
                        for(int j=0;j<5;j++)
                        {
                            Console.Write(span[i * 5 + j]);
                            count++;
                        }
                        Console.Write("\n");
                    }
                    return count;
                }
                else
                {
                    return 0;
                }
            });
            a.errorEvent=(UserIOError u) => { Console.WriteLine(u.ToString()); Root.root.UserIOPool.RepayBlock(a); };
            a.ReciveStart();
        }
        public class abc
        {
            int a, b, c;
        }
        public class dfg:abc
        {
            int r, g, s;
        }
    }
}

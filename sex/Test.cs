using sex.Conversion;
using sex.DataStructure;
using sex.Networking;
using sex.Pooling;
using sex.UserDefinedNetPacket;
using System.Collections.Generic;
using System.Diagnostics;
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
            //INetStructTest();
            //DecoderTest();
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
        static unsafe void DecoderTest()
        {
            var table = Root.root.table;

            int count = 0;
            ConvertibleGroup group = new ConvertibleGroup(table);
            EnDecoder enDecoder = new EnDecoder(group,
                (INetConvertible data) =>{
                    Vecter3Int mydata = (Vecter3Int)data;
                    count++;
                    group.ReturnBlock(data);
                }
            );


            Stopwatch sw = new Stopwatch();

            DynamicBuff<byte> myBuff = new DynamicBuff<byte>(new byte[1000000]);
            Vecter3Int myData = new Vecter3Int(0, 0, 0);
            sw.Start();
            for (int c = 0; c < 100; c++)
            {
                for (int i = 0; i < 100000; i++)
                {
                    myData.y = i;
                    if(enDecoder.Encode(myBuff, myData))
                    {

                    }
                    else
                    {
                        break;
                    }
                    //if(myBuff.Write(3,out Span<byte>span))
                    //{
                    //    span[0] = 0b11111111;
                    //    span[1] = 0b11111111;
                    //    span[2] = 0b11111111;
                    //}
                }
                //Console.WriteLine("ssssssssssssss");
                int nByteProcessed = 0;
                if (myBuff.NonCountingRead(out Span<byte> span))
                {
                    nByteProcessed = enDecoder.Decode(span);
                    myBuff.IncreaseReadOffset(nByteProcessed);
                }
            }
            sw.Stop();
            Console.WriteLine("rgrgL " + count + " time:" + sw.ElapsedMilliseconds);
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

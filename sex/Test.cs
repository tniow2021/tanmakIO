using sex.Networking;
using sex.Pooling;
using System;
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
            PoolTest();
            UserIOTest();
        }
        public static void PoolTest()
        {
            PoolEngine pm = Root.root.poolEngine;
            pm.PrintPoolStatistics();
            Func<abc> aa = () => { return new abc(); };
            for (int i = 0; i < 30; i++)
            {
                IPool<abc> abcPool = Root.root.poolEngine.CreatePool<abc>(aa, 4000, 2 + i);
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
            a.SetUserIO(client, 9,packetSizeLimit:1024);
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

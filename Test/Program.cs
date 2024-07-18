using sex.Networking;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using Test;
public class AA
{
    
        public int a;
}
static class Program
{
    static Socket server;
    static Socket ServerClient;
    static Socket client;
    static SocketAsyncEventArgs args;
 
    static void Main()
    {
        Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sk.Connect(new IPEndPoint(IPAddress.Loopback, 2024));

        UserIO userIO = new UserIO();
        userIO.Assemble();
        NetPacketPort divider =
            new NetPacketPort(Root.root.NetPacketMinimumLengthTable, null);
        userIO.SetUserIO(sk, 1024, divider.Decode);
        userIO.recieveEvent = divider.Decode;
        this.divider = divider;
        this.userIO = userIO;
        this.localID = -1;
    }













    public abstract class II
    {
        public static short n;
    }
    class AA:II
    {
        public int a;
    }
    static void Mai444n()
    {
        string s = "";
        while (true)
        {
            s=Console.ReadLine();
            if (s == "sex")
                break;
            Console.WriteLine("하아 언조비카이 " + s);
        }
    }
    static void M4ain()
    {
        int[] a = new int[] { 0,1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var buff=new Test.NonContiguousBuffer<int>(a,8,9,3,0);
        for(int i=0;i<buff.length;i++)
        {
            Console.WriteLine(buff[i] + " .");
        }
    }
    static void Mai3n()
    {
        Stopwatch sw = new Stopwatch();
        AA aaa=new AA();
        StackPool<AA> p = new StackPool<AA>(() => { return aaa; } ,100);

        sw.Start();
        for(int i=0;i<1000000;i++)
        {
            p.RepayBlock(p.GetBlock());
            p.RepayBlock(p.GetBlock());
            p.RepayBlock(p.GetBlock());
            p.RepayBlock(p.GetBlock());
            p.RepayBlock(p.GetBlock());
            p.RepayBlock(p.GetBlock());
        }
        sw.Stop();
        Console.WriteLine("test1 " + sw.ElapsedMilliseconds);
        sw.Reset();

        QPool<AA>p2=new QPool<AA>(() => { return aaa; }, 100);
        sw.Start();
        for (int i = 0; i < 1000000; i++)
        {
            p2.RepayBlock(p2.GetBlock());
            p2.RepayBlock(p2.GetBlock());
            p2.RepayBlock(p2.GetBlock());
            p2.RepayBlock(p2.GetBlock());
            p2.RepayBlock(p2.GetBlock());
            p2.RepayBlock(p2.GetBlock());
        }
        sw.Stop();
        Console.WriteLine("test1 " + sw.ElapsedMilliseconds);
        sw.Reset();
    }
    static void Main5()
    {
        Stopwatch sw = new Stopwatch();
        AA aaa = new AA();
        StackPool<AA> p = new StackPool<AA>(() => { return aaa; }, 100);
        for (int i = 0; i < 20; i++)
        {
            p.GetBlock();
            Console.WriteLine("count: " + p.count);
            p.Display();
        }
        for (int i=1;i<11;i++)
        {
            AA a=new AA();
            a.a = i;
            p.RepayBlock(a);
            p.Display();
        }
        for (int i = 1; i < 15; i++)
        {
            p.GetBlock().a=0;
            Console.WriteLine("count: " + p.count);
            p.Display();
        }
        for (int i = 1; i < 16; i++)
        {
            AA a = new AA();
            a.a = i;
            p.RepayBlock(a);
            p.Display();
        }
    }
    static void Main4()
    {
        
        object temp = new object();
        int temp3 = 0;

        Stopwatch sw = new Stopwatch();

        int temp2=0;
        sw.Start();
        for (int i = 0; i < 1000000; i++)
        {
            Interlocked.Increment(ref temp2);
        }
        sw.Stop();
        Console.WriteLine("test1 " + sw.ElapsedMilliseconds+" "+temp2);
        sw.Reset();


        sw.Start();
        for(int i=0;i<1000000;i++)
        {
            lock (temp)
            {
                temp3++;
            }
        }
         sw.Stop();
        Console.WriteLine("test1 " + sw.ElapsedMilliseconds);
        sw.Reset();
    }
    //static void Main3()
    //{
    //    Stopwatch sw = new Stopwatch();
    //    float a=3;
    //    bool b = true;
    //    object temp = new object();
    //    t1qPool<object> t1 = new Test.t1qPool<object>(() => { return temp; }, 1000000);
    //    t2qPool<object> t2 = new Test.t2qPool<object>(() => { return temp; }, 1000000);

    //    long tt1 = 0;
    //    long tt2 = 0;
    //    for(int kk=0;kk<20;kk++)
    //    {
    //        if(kk%2==0)
    //        {
    //            sw.Start();
    //            for (int i = 0; i < 1000000; i++)
    //            {
    //                t2.RepayBlock(t2.GetBlock());
    //                t2.RepayBlock(t2.GetBlock());
    //                t2.RepayBlock(t2.GetBlock());
    //                t2.RepayBlock(t2.GetBlock());
    //                t2.RepayBlock(t2.GetBlock());
    //                t2.RepayBlock(t2.GetBlock());
    //            }
    //            sw.Stop();
    //            Console.WriteLine("tt2 "+sw.ElapsedMilliseconds);
    //            tt2 += sw.ElapsedMilliseconds;
    //            sw.Reset();
    //        }
    //        else
    //        {
    //            sw.Start();
    //            for(int i=0;i<1000000;i++)
    //            {
    //                t1.GetBlock();
    //                t1.GetBlock();
    //                t1.GetBlock();
    //                t1.RepayBlock(temp);
    //                t1.RepayBlock(temp);
    //                t1.RepayBlock(temp);
    //            }
    //            sw.Stop();
    //            Console.WriteLine("tt1 "+sw.ElapsedMilliseconds);
    //            tt1 += sw.ElapsedMilliseconds;
    //            sw.Reset();
    //        }
    //    }




    //    Console.WriteLine("sw.ElapsedMilliseconds" + tt1 + "  " + tt2); ;
    //}
    static void Mainw()
    {
        //qPool<AA> p = new qPool<AA>(() => { return new AA(); }, 9);
        //for(int i=0;i<200;i++)
        //{
        //    for(int j=0;j<3;j++)
        //    {
        //        AA a1=p.GetBlock();
        //         p.Display();

        //    }

        //    Console.WriteLine("");
        //    for (int j = 0; j < 4; j++)
        //    {
        //    p.RepayBlock(new AA());
        //    p.Display();
        //    }
        //    Console.WriteLine("\n");

        Stopwatch sw = new Stopwatch();
        QPool<int> p = new QPool<int>(() => { return 1; }, 1000000);

        Thread t1 = new Thread(
            () =>
            {
                for(int i=0;i<400000;i++)
                {
                    lock (p)
                    {
                        p.GetBlock();
                    }
                    //p.GetBlock();
                }
            });
        Thread t2 = new Thread(
            () =>
            {
                for (int i = 0; i < 400000; i++)
                {
                    lock (p)
                    {
                        p.GetBlock();
                    }
                    //p.GetBlock();
                }
            });
        sw.Start();
        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
        sw.Stop();
        Console.WriteLine("nspace: " + p.nSpace+"  time: "+sw.ElapsedMilliseconds);
    }


        //Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //s.Connect(new IPEndPoint(IPAddress.Loopback, 20240));
        //Console.WriteLine("연결됨");
        //byte[] t = new byte[100];
        //for(int i=0;i<30000000;i++)
        //{ 
        //    Console.ReadLine();
        //    //s.Send(System.Text.Encoding.Default.GetBytes("sefsefsㄴㄹㄴㄹㄴsef" + i + "sfvsvsev"));
        //    for(byte i2=0;i2<100;i2++)
        //    {
        //        t[i2] =i2;
        //    }
        //    s.Send(t);
        //    Console.WriteLine("보냄");
    
}


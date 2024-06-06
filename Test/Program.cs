using System.Net;
using System.Net.Sockets;
using Test;
static class Program
{
    static Socket server;
    static Socket ServerClient;
    static Socket client;
    static SocketAsyncEventArgs args;
    public class A
    {
        public int a;
    }
    
    static void Main()
    {
        qPool<A> p = new qPool<A>(() => { return new A(); }, 10);
        for(int i=0;i<200;i+=3)
        {
            A a1=p.GetBlock();
            a1.a = i;

            A a2 = p.GetBlock();
            a2.a = i+1;

            A a3 = p.GetBlock();
            a3.a = i+2;
            A a4 = p.GetBlock();
            a1.a = i;

            A a5 = p.GetBlock();
            a2.a = i + 1;

            A a6 = p.GetBlock();
            a3.a = i + 2;

            p.RepayBlock(a1);
            p.RepayBlock(a2);
            p.RepayBlock(a3);
            p.RepayBlock(a4);
            p.RepayBlock(a5);
            p.RepayBlock(a6);

            Console.WriteLine("");
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
}


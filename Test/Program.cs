using System.Net;
using System.Net.Sockets;

static class Program
{
    static Socket server;
    static Socket ServerClient;
    static Socket client;
    static SocketAsyncEventArgs args;
    static void Main2()
    {

        server=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Any, 20240));
        server.Listen(10);

        args= new SocketAsyncEventArgs();
        args.Completed += new EventHandler<SocketAsyncEventArgs>(aaaa);
        args.SetBuffer(new byte[1000],0,999);
        ServerClient=server.Accept();
        wwww();

        Thread.Sleep(100000);
    }

    static void wwww()
    {
        bool pending = false;
        while (!pending)
        {
            pending=ServerClient.ReceiveAsync(args);
            //검사를 거쳐야 무한루프를 피함.
            Console.WriteLine("tlqkf");
            if (!pending)
            {
                Console.WriteLine("AS");
                aaaa(null,args);
            }
        }
        
        
    }

    static void aaaa(object s, SocketAsyncEventArgs args)
    {
        Console.WriteLine("홓");
        Console.WriteLine($"count:{args.Count}, bytefeee:{args.BytesTransferred}");
        if (args.Buffer !=null)
            Console.WriteLine(System.Text.Encoding.Default.GetString(args.Buffer,args.Offset,args.Count)+"swx");
        else
        {
            Console.WriteLine("sex2");
        }
        if (args.BufferList != null) Console.WriteLine("아주그냥 섹스");
        wwww();
    }

}
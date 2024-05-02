
using sex.Networking;
using System.Net.Sockets;

public static class Program
{
    static int Main()
    {
        Accepter accepter = new Accepter(2024, socketConnectEvent);
        return 0;
    }
    static void socketConnectEvent(Socket sk)
    {
        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.SetBuffer(new byte[1024]);
        args.Completed += (object sender, SocketAsyncEventArgs _args) =>
        {

        };
        sk.ReceiveAsync(args);
    }
}
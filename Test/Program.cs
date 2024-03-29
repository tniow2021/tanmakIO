using System.Net;
using System.Net.Sockets;

static class Program
{
    static void Main()
    {
        Socket socket = new Socket(SocketType.Stream,ProtocolType.Tcp);
        Console.WriteLine("접속대기중");
        socket.Connect(new IPEndPoint(IPAddress.Loopback, 2024));
        Console.WriteLine("접속함");

        Console.ReadLine();
        socket.Close();
    }
}
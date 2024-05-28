using System.Net;
using System.Net.Sockets;
class Program
{
    static void Main()
    {
        Socket client=new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(new IPEndPoint(IPAddress.Loopback, 20240));
        Console.WriteLine("연결?");
        for(int i=0; true;i++)
        {
            client.Send(System.Text.Encoding.Default.GetBytes("tlqkgtlqkgtlqkf"+i.ToString()));
            Thread.Sleep(2000);

        }
    }
}
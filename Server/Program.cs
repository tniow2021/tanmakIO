using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

static class Program
{
    static void Main()
    {
        Console.WriteLine("프로그램 시작");
        Server server = new Server(_port:2024,_maxUser:1000);
        while (true)
        {
            int onlineUserCount= server.Update();
            if(onlineUserCount <= 0) 
            {
                Thread.Sleep(100);
            }
        }
    }
}
class Server
{
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //(추후) 최적화
    List<Socket> clients = new List<Socket>();
    int maxUser = 0;
    public Server(int _port, int _maxUser)
    {
        maxUser= _maxUser;
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
        serverSocket.Listen(100);
        serverSocket.Blocking = false;
    }
    public int Update()
    {
        return clients.Count;
    }


    string Receive(byte[] data,int n)
    {
        string msg = Encoding.UTF8.GetString(data, 0, n);
        Console.WriteLine(msg);
        return msg;
    }
    void Send(string msg)
    {
        
    }
}
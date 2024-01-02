using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

static class Program
{
    static void Main()
    {
        Console.WriteLine("프로그램 시작");
        Server server = new Server(_port:2024);
        server.Start(1);
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
    public Server(int _port)
    {
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
        serverSocket.Listen(100);
    }
    public void Start(int maxUser)
    {
        Console.WriteLine("접속받는 중...현재:{0}명", clients.Count);
        while (clients.Count < maxUser)
        {
            Socket client=serverSocket.Accept();
            clients.Add(client);
            Console.WriteLine("접속받는 중...현재:{0}명", clients.Count);
        }
    }
    public int Update()
    {
        //듣기
        byte[]buff = new byte[1024];
        foreach (Socket client in clients)
        {
            int n=client.Receive(buff);
            string msg= Receive(buff, n);

            client.Send(Encoding.UTF8.GetBytes("냥냥이"));
        }
        //보내기(추후)
        //출력하기

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
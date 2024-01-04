using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

//https://siku314.tistory.com/75
//https://sanghun219.tistory.com/104
static class Program
{
    static void Main()
    {

        myEchoServer();
    }
    static void myEchoServer()
    {
        EchoServer echoServer = new EchoServer(2024, 1000);
        while(true)
        {
            Thread.Sleep(10);
            echoServer.Update();
        }
    }
    static void myServer()
    {
        Console.WriteLine("프로그램 시작");
        Server server = new Server(_port: 2024, _maxUser: 1000);
        while (true)
        {
            int onlineUserCount = server.Update();
            if (onlineUserCount <= 0)
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
    SocketAsyncEventArgs AcceptArgs;
    public Server(int _port, int _maxUser)
    {
        maxUser= _maxUser;
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
        serverSocket.Listen(100);

        //init //버퍼 와 애그(들)을 만들고 애그에 대리자를 등록시켜준다.
        AcceptArgs = new SocketAsyncEventArgs();
        AcceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);
    }
    public int Update()
    {
        //접속받기
        bool Pending= serverSocket.AcceptAsync(AcceptArgs);
        //Pending(처리중)이 참이면 이벤트로 처리하고, 거짓이면 매개변수로 넣은 애그를 직접 뒤진다.
        if (Pending is false)
        {
            OnRecvCompleted(null, AcceptArgs);
        }


        //I/O 이벤트 넣어주기

        return clients.Count;
    }
    

    public void OnRecvCompleted(object sender,SocketAsyncEventArgs 접속애그)
    {
        if(접속애그.SocketError==SocketError.Success)
        {
            clients.Add(접속애그.AcceptSocket);
        }
    }
}
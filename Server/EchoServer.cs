using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;


class EchoServer
{
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //(추후) 최적화
    List<Socket> clients = new List<Socket>();
    int maxUser = 0;
    public EchoServer(int _port, int _maxUser)
    {
        maxUser = _maxUser;
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
        serverSocket.Listen(100);
        Init();
        AcceptStart();
    }
    SocketAsyncEventArgs AcceptArg=new SocketAsyncEventArgs();
    void Init()
    {
        AcceptArg.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_completed);
        AcceptArg.SetBuffer(new byte[1024], 0, 1024);
    }
    byte[] recBuff = new byte[1024];
    public void Update()
    {
        foreach(var client in clients)
        {
            int canReadByteCount = client.Available;
            if(canReadByteCount > 0 )
            {
                if (canReadByteCount > 1024) Console.WriteLine("이게 뭔일임. 버퍼에 1024나 쌓임");
                int n= client.Receive(recBuff);
                if (n != canReadByteCount) Console.WriteLine("으악 Available하고 실제 읽은 바이트 수가 다르다.");
                
                client.Send(recBuff,canReadByteCount,SocketFlags.None);
            }
        }
    }
    void AcceptStart()//비동기로 재귀
    {
        Console.WriteLine("연결받는중...현재{0}명",clients.Count);
        AcceptArg.AcceptSocket = null;
        if (serverSocket.AcceptAsync(AcceptArg) is false)
        {
            Accept_completed(null, AcceptArg);
        }
    }
    void Accept_completed(object sender, SocketAsyncEventArgs e)
    {
        if(e.SocketError==SocketError.Success)
        {
            lock(clients)
            {
                clients.Add(e.AcceptSocket);
            }
        }
        else
        {
            Console.WriteLine("소켓에러");
        }
        AcceptStart();
    }
}

class Delimiter
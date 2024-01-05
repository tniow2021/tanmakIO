using System.Net.Sockets;
using System.Net;

class Server
{
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //(추후) 최적화
    List<ClientNetwork> clientNetworks = new List<ClientNetwork>();
    int maxUser = 0;

    SocketAsyncEventArgs AcceptArgs = new SocketAsyncEventArgs();
    public Server(int _port, int _maxUser)
    {
        maxUser = _maxUser;
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
        serverSocket.Listen(100);
        Init();
        Start();
    }
    void Init()
    {
        AcceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_completed);
        AcceptArgs.SetBuffer(new byte[1024], 0, 1024);
    }
    void Start()
    {
        AcceptStart();
    }
    public int Update()
    {
        for(int i = 0; i < clientNetworks.Count;i++)
        {
            ClientNetwork network = clientNetworks[i];
            //나중에연결해제 처리
            network.Update();
            if(network.typeBuff.pull(out INetStruct ns,TypeCode.UserTransform))
            {
                SendToAllClinet(ns);
                UserTransform u = (UserTransform)ns;
                Console.WriteLine(u.x+":"+u.y);
            }
        }
        return clientNetworks.Count;
    }
    void SendToAllClinet(INetStruct ns)
    {
        for (int i = 0; i < clientNetworks.Count; i++)
        {
            ClientNetwork network = clientNetworks[i];
            //나중에연결해제 처리
            network.typeBuff.Push(ns);
        }
    }
    void AcceptStart()//비동기로 재귀
    {
        Console.WriteLine("연결받는중...현재{0}명", clientNetworks.Count);
        AcceptArgs.AcceptSocket = null;
        if (serverSocket.AcceptAsync(AcceptArgs) is false)
        {
            Accept_completed(null, AcceptArgs);
        }
    }
    void Accept_completed(object sender, SocketAsyncEventArgs e)
    {
        if (e.SocketError == SocketError.Success)
        {
            lock (clientNetworks)
            {
                clientNetworks.Add(new ClientNetwork(e.AcceptSocket));
            }
        }
        else
        {
            Console.WriteLine("소켓에러");
        }
        AcceptStart();
    }
}
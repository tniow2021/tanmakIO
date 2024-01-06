using System.Net.Sockets;
using System.Net;
using System.Timers;
using System.Diagnostics;

class Server
{
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //(추후) 최적화
    List<ClientNetwork> clientNetworks = new List<ClientNetwork>();
    int maxUser = 0;

    SocketAsyncEventArgs AcceptArgs = new SocketAsyncEventArgs();
    UserManager userManager = new UserManager();

    System.Timers.Timer timer = new System.Timers.Timer();
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
        timer.Interval = 1000;
        timer.Elapsed += TimerEvent;
    }
    int deltaTime = 0;
    void TimerEvent(Object source, System.Timers.ElapsedEventArgs e)
    {
        deltaTime = 1;
    }
    void Start()
    {
        AcceptStart();
        timer.Start();
    }
    public int Update()
    {
        Console.WriteLine("시간:" + deltaTime);
        for (int i = 0; i < clientNetworks.Count;i++)
        {
            ClientNetwork network = clientNetworks[i];
            //나중에연결해제 처리를 받으면
            bool IsConnect = network.Update(deltaTime: deltaTime);
            if (IsConnect is false)
            {
                Console.WriteLine("시발");
                clientNetworks.Remove(network);
                continue;
            }
            //AccessRequest를 받으면
            while (network.typeBuff.pull(out INetStruct ns,TypeCode.AccessRequest))
            {
                AccessRequest ar = (AccessRequest)ns;
                Console.WriteLine("사용자가 AccessRequest를 보내옴");
                int id= userManager.CreateUser(network);
                network.typeBuff.Push(new AccessRequestAnswer(id));
            }
            //UserTransform를 받으면
            while (network.typeBuff.pull(out INetStruct ns, TypeCode.UserTransform))
            {
                SendToAllClinet(ns);
                UserTransform u = (UserTransform)ns;
            }
            //DummyData
            while (network.typeBuff.pull(out INetStruct ns, TypeCode.DummyData))
            {
                //notthing
            }

        }
        Console.WriteLine(clientNetworks.Count);
        deltaTime = 0;
        return clientNetworks.Count;
    }
    void SendToAllClinet(INetStruct ns)
    {
        for (int i = 0; i < clientNetworks.Count; i++)
        {
            ClientNetwork network = clientNetworks[i];
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
                clientNetworks.Add(new ClientNetwork(e.AcceptSocket,new TypeBuff()));
            }
        }
        else
        {
            Console.WriteLine("소켓에러");
        }
        AcceptStart();
    }
}
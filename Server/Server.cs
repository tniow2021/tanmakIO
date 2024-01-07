using System.Net.Sockets;
using System.Net;

class Server
{
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    //(추후) 최적화
    List<ClientNetwork> clientNetworks = new List<ClientNetwork>();
    int maxUser = 0;

    SocketAsyncEventArgs AcceptArgs = new SocketAsyncEventArgs();
    UserManager userManager = new UserManager();
    //서버소켓을 엽니다
    public Server(int _port, int _maxUser)
    {
        maxUser = _maxUser;
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
        serverSocket.Listen(100);
        Init();
        Start();
    }
    //필요한 설정을 초기화합니다
    void Init()
    {
        AcceptArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_completed);
        AcceptArgs.SetBuffer(new byte[1024], 0, 1024);
    }
    //Start()는 AcceptStart()를 실행할 뿐입니다..
    void Start()
    {
        AcceptStart();
    }
    //network속에 typebuff에서 데이터를 가져와 가장 중요한 게임로직을 처리합니다
    public int Update()
    {
        for (int i = 0; i < clientNetworks.Count;i++)
        {
            ClientNetwork network = clientNetworks[i];
            bool IsConnect = network.Update();
            if (IsConnect is false)
            {
                if(userManager.GetUser(network,out User user))
                {
                    Console.WriteLine("ID:"+ user.ID+ "유저의 연결이 끊어짐");
                    network.typeBuff.Push(new ExitUserSignal(user.ID));
                    userManager.RemoveUser(user);
                }
                clientNetworks.Remove(network);
                Console.WriteLine("현재총:"+clientNetworks.Count+"명");
                continue;
            }
            //AccessRequest를 받으면
            while (network.typeBuff.pull(out INetStruct ns,TypeCode.AccessRequest))
            {
                AccessRequest ar = (AccessRequest)ns;
                Console.WriteLine("사용자가 AccessRequest를 보내옴");
                int id= userManager.CreateUser(network);
                network.typeBuff.Push(new AccessRequestAnswer(id));
                Console.WriteLine("아이디:" + id + "부여.");
            }
            //UserTransform를 받으면
            while (network.typeBuff.pull(out INetStruct ns, TypeCode.UserTransform))
            {
                var u=(UserTransform)ns;
                //Console.WriteLine("iii" + u.ID);
                SendToAllClinet(ns);
            }
            //DummyData
            while (network.typeBuff.pull(out INetStruct ns, TypeCode.DummyData))
            {
                //notthing
            }

        }
        //Console.WriteLine(clientNetworks.Count);
        return clientNetworks.Count;
    }
    //모두에게 특정 정보를 송신하고 싶을때 사용합니다
    void SendToAllClinet(INetStruct ns)
    {
        for (int i = 0; i < clientNetworks.Count; i++)
        {
            ClientNetwork network = clientNetworks[i];
            network.typeBuff.Push(ns);
        }
    }
    //아래 두함수는 비동기적으로 서로를 호출하며 소켓접속을 받습니다
    void AcceptStart()
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
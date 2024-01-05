using System.Net;

public class GameManager
{
    /*
     * 
     */
    public static GameManager Instance { get; private set; }
    ClientNetwork network;
    public static TypeBuff GetTypeBuff() { return Instance.network.typeBuff; }
    public GameManager() 
    {
        if (Instance is not null) return;
        Instance = this;
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        //네트워크 객체에 타입버퍼 객체를 넣어준다.
        network = new ClientNetwork(ServerIP, port);
    }
    public void Update()
    {
        network.Update();
    }
}

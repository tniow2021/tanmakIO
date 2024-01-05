using System.Net;

public class GameManager
{
    /*
     * 
     */
    public static GameManager Instance { get; private set; }
    Network network;
    public TypeBuff typeBuff { get; private set; }
    public static TypeBuff GetTypeBuff() { return Instance.typeBuff; }
    public GameManager() 
    {
        if (Instance is not null) return;
        Instance = this;
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        //네트워크 객체에 타입버퍼 객체를 넣어준다.
        typeBuff = new TypeBuff(network);
        network = new Network(ServerIP, port,typeBuff);
    }
    public void Update()
    {
        network.Update();
    }
}

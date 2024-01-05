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
        //��Ʈ��ũ ��ü�� Ÿ�Թ��� ��ü�� �־��ش�.
        typeBuff = new TypeBuff(network);
        network = new Network(ServerIP, port,typeBuff);
    }
    public void Update()
    {
        network.Update();
    }
}

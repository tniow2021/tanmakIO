using System.Net.Sockets;
using System.Net;
public class ClientNetwork
{
    /*
     * ������ �۽�:
     * 1.�ƹ� ��ũ��Ʈ�� �����͸� Converting.To~~()�Լ��� ���� INetStruct������ ����ü�� �����.
     * 2.INetStruct ������ ����ü�� Network.Send()�Լ��� ������
     * 3. Network.Send �Լ��� ����ü�� ���ڵ��� �� binaryHandler�� ��ŷ�Ѵ�.
     * 4. binaryHandler�� ������ ���Ĵ�� ��ŷ�� ����Ʈ���� �������� �۽��Ѵ�.
     * 
     * ������ ����:
     * 1. void ReceiveUpdate()�� ���鼭 �������� �޴´�
     * 2. binaryHandler��ü�� �����͸� �����ϴµ� �����Ͱ� ����� �׿��� True�� ��ȯ�Ѵ�.
     *    (�̶� outŰ����� ���̳ʸ��� ��ȯ�ȴ�.)
     * 3. ����Ʈ���� typeBuff�� push()���ش�. 
     * 4. typeBuff�� push�� �����ʹ� ����ü�� ���ڵ��Ǿ� typeBuff���� ť�� ����ȴ�.
     * 5. �ƹ� ��ũ��Ʈ�� typeBuff.pull(����ü�� Ÿ���ڵ�)�� ���� ���ϴ� ����ü�� ��ȯ���� �� �ִ� 
     */
    //�翬��� ��Ό�ܰ����� ���߿� �����غ���.

    public Socket client;
    IPEndPoint ServerIP;
    int port;
    bool IsConnect=false;

    public TypeBuff typeBuff { get; private set; }
    public ClientNetwork(IPAddress connectAddress, int _port)
    {
        typeBuff = new TypeBuff(this);
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port = _port;
        
        //Start
        client.Connect(ServerIP);
        IsConnect = true;
        //MonoBehaviour.print("���Ἲ��");
    }
    public ClientNetwork(Socket _clinet)
    {
        typeBuff = new TypeBuff(this);
        client = _clinet;
        IsConnect = true;
    }


    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    void Send()
    {
        while(typeBuff.BinaryPull(out byte[] data))
        {
            int a= client.Send(binaryHandler.Pack(data),SocketFlags.None,out SocketError error);
            if(error == SocketError.SocketError)
            {
                client.Close();
                IsConnect = false;
            }
        }
    }
    public bool Update()
    {
        Receive();
        Send();

        
        return IsConnect;
    }
    void Receive()
    {
        while (client.Available>0)
        {
            byte[] buff = new byte[client.Available];
            client.Receive(buff, buff.Length, SocketFlags.None);
            foreach(byte b in buff)
            {
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1����Ʈ�� ������ �����̽��� �� true�� �Բ� out.
                {
                    typeBuff.BinaryPush(binarySplited);//�ٽ�
                }
            }
        }
    }
}
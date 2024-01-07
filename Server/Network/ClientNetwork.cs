using System.Net.Sockets;
using System.Net;
public class ClientNetwork
{
    public Socket client { get; private set; }
    IPEndPoint ServerIP;
    int port;
    bool IsConnect=false;
    public TypeBuff typeBuff { get; private set; }
    public ClientNetwork(IPAddress connectAddress, int _port, TypeBuff _typeBuff)
    {
        typeBuff = _typeBuff;
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port = _port;
        
        //����
        client.Connect(ServerIP);
        IsConnect = true;
        //MonoBehaviour.print("���Ἲ��");
    }
    public ClientNetwork(Socket _clinet, TypeBuff _typeBuff)
    {
        typeBuff = _typeBuff;
        client = _clinet;
        IsConnect = true;
    }
    int waittingCount = 0;//Update�� 1000�� �� ������ ���̸� �ϳ��� ������ ������¸� üũ�Ѵ�.
    public bool Update()
    {
        if(IsConnect&&client.Connected)
        {
            Receive();//tcp������ ��ŭ �����Ͱ� ���������� ���� ������ ������ ������ �� �ִ�.
            Send();
        }

        waittingCount++;
        if(waittingCount >=1000)
        {
            waittingCount = 0;
            typeBuff.Push(new DummyData());
        }
        return IsConnect;
    }//Send()�� Receive() ȣ��. �� �Լ��� ��� ȣ���ؾ� ������� �̷���.
    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    void Send()
    {
        int count=0;
        while (typeBuff.BinaryPull(out byte[] data))
        {
            count= client.Send(binaryHandler.Pack(data),SocketFlags.None,out SocketError error);
            if (error != SocketError.Success)
            {
                Console.WriteLine("����. ��������.");
                client.Close();
                IsConnect = false;
            }
            if(count==0)
            {
                IsConnect= false;
                client.Close();
            }
        }
    }//typeBuff.BinaryPull(out byte[] data)�ؼ� ���� �����͸� ������ �۽�
    void Receive()
    {
        int count = 0;
        while (client.Available>0)
        {
            byte[] buff = new byte[client.Available];
            count+= client.Receive(buff, 0,buff.Length, SocketFlags.None,out SocketError error);
            if (error != SocketError.Success)
            {
                Console.WriteLine("����. ��������.");
                client.Close();
                IsConnect = false;
                return;
            }
            foreach (byte b in buff)
            {
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1����Ʈ�� ������ �����̽��� �� true�� �Բ� out.
                {
                    typeBuff.BinaryPush(binarySplited);//�ٽ�
                }
            }
        }
        return;
    }//typeBuff.BinaryPush(������)�Ͽ� ������ �����͸� ������ ����
}
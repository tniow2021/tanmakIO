using System.Net.Sockets;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

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
    public ClientNetwork(IPAddress connectAddress, int _port, TypeBuff _typeBuff)
    {
        typeBuff = _typeBuff;
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port = _port;
        
        //Start
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
    int waittingCount = 0;//1000���� ���� ���̸� �ϳ��� ������ ���¸� üũ�Ѵ�.
    public bool Update()
    {
        if(IsConnect&&client.Connected)
        {
            Receive();//tcp������ ��ŭ �����Ͱ� ���������� ���� ������ ������ ������ �� �ִ�.
            Send();
        }
        //Console.WriteLine("���ú����:" + typeBuff.recieveQueues[0].Count);
        //Console.WriteLine("�������:" + typeBuff.SendQueues.Count);
        waittingCount++;
        if(waittingCount >=1000)
        {
            waittingCount = 0;
            typeBuff.Push(new DummyData());
        }
        return IsConnect;
    }
    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    void Send()
    {
        int count=0;
        while (typeBuff.BinaryPull(out byte[] data))
        {
            count= client.Send(binaryHandler.Pack(data),SocketFlags.None,out SocketError error);
            //Console.WriteLine(count);
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
            //Console.WriteLine("����������:" + count);
        }
    }

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
    }
}
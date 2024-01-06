using System.Net.Sockets;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
    public bool Update(float deltaTime)
    {
        if(IsConnect)
        {
            Receive();//tcp������ ��ŭ �����Ͱ� ���������� ���� ������ ������ ������ �� �ִ�.
            Send();
            ConnectCherk(deltaTime);
        }
        
        //Console.WriteLine("���ú����:" + typeBuff.recieveQueues[0].Count);
        //Console.WriteLine("�������:" + typeBuff.SendQueues.Count);
        return IsConnect;
    }

    public float timeOutTime = 20;//������ TimeOutCherk���κ��� ��ٸ� �� �ִ� �ð�.
    public float repeadWattingInterval = 10;//TimeOutCherk ���� ���ð�
    float timeOutTime_count = 0;//Ÿ�̸�
    float repeadWattingInterval_count = 0;//Ÿ�̸�
    bool ConnectCherk(float deltaTime)
    {
        //�����ֱ�� ���� ����
        if (repeadWattingInterval_count < repeadWattingInterval)
        {
            repeadWattingInterval_count += deltaTime;
        }
        else//�ð� �ʰ���
        {
            typeBuff.Push(new TimeOutCherk());
        }
        //timeOut�� ���� �ܰ�
        timeOutTime_count += deltaTime;
        Console.WriteLine("tl:" + timeOutTime_count+":"+ deltaTime);
        while (typeBuff.pull(out INetStruct ns, TypeCode.TimeOutCherk))
        {
            timeOutTime_count = 0;
        }
        if (timeOutTime_count > timeOutTime)//�ð��ʰ���
        {
            IsConnect = false;
            client.Close();
            return false;
        }
        return true;
    }
    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    int Send()
    {
        int count=0;
        while (typeBuff.BinaryPull(out byte[] data))
        {
            count+= client.Send(binaryHandler.Pack(data),SocketFlags.None,out SocketError error);
            //Console.WriteLine(count);
            if (error != SocketError.Success)
            {
                Console.WriteLine("����. ��������.");
                client.Close();
                IsConnect = false;
            }
            //Console.WriteLine("����������:" + count);
        }
        return count;
    }
    float timeOutCount = 0;

    void Receive()
    {
        int count = 0;
        while (client.Available>0)
        {
            Console.WriteLine("��");
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
                Console.WriteLine(".." + b);
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1����Ʈ�� ������ �����̽��� �� true�� �Բ� out.
                {
                    typeBuff.BinaryPush(binarySplited);//�ٽ�
                }
            }
        }
        return;
    }
}
using System.Net.Sockets;
using System.Net;
public class ClientNetwork
{
    public Socket client { get; private set; }
    IPEndPoint ServerIP;
    int port;
    public bool IsConnect { get; private set; } = false;

    public TypeBuff typeBuff { get; private set; }
    public ClientNetwork(IPAddress connectAddress, int _port, TypeBuff _typeBuff)
    {
        typeBuff = _typeBuff;
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port = _port;
        
        
    }
    public ClientNetwork(Socket _clinet)
    {
        typeBuff = new TypeBuff();
        client = _clinet;
        IsConnect = true;
    }

    bool success2 = false;
    public bool ConnectStart(int second)
    {
        System.IAsyncResult result;
        result = client.BeginConnect(ServerIP, ConnectCompletedEvent, null);
        bool success = result.AsyncWaitHandle.WaitOne(second*1000, false);
        success2 = false;
        if (success)
        {
            if(success2)//���ð��� �ٰ������� �񿬰������ ��ȯ�Ǵ� ���� �־��⶧���� �߰�.
            {
                client.EndConnect(result);
                IsConnect = false;
                return false;
            }
            
        }
        client.Close();
        IsConnect = false;
        return false;
    }
    void ConnectCompletedEvent(System.IAsyncResult ar)
    {
        if(ar.CompletedSynchronously)//����Ǿ�����
        {
            success2 = true;
        }
        else
        {
            success2 = false;
        }
    }
    public bool Update()
    {
        if(IsConnect)
        {
            Receive();
            Send();
        }
        
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
    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    void Send()
    {
        while(typeBuff.BinaryPull(out byte[] data))
        {
            int a= client.Send(binaryHandler.Pack(data),SocketFlags.None,out SocketError error);
            if(error != SocketError.Success)
            {
                client.Close();
                IsConnect = false;
            }
        }
    }
}
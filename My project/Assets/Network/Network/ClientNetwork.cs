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
        
        //Start
        client.Connect(ServerIP);
        IsConnect = true;
    }
    public ClientNetwork(Socket _clinet)
    {
        typeBuff = new TypeBuff();
        client = _clinet;
        IsConnect = true;
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
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1바이트씩 보내면 슬라이스될 때 true와 함꼐 out.
                {
                    typeBuff.BinaryPush(binarySplited);//핵심
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
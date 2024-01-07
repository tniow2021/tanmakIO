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
        
        //연결
        client.Connect(ServerIP);
        IsConnect = true;
        //MonoBehaviour.print("연결성공");
    }
    public ClientNetwork(Socket _clinet, TypeBuff _typeBuff)
    {
        typeBuff = _typeBuff;
        client = _clinet;
        IsConnect = true;
    }
    int waittingCount = 0;//Update가 1000번 돌 때마다 더미를 하나씩 보내어 연결상태를 체크한다.
    public bool Update()
    {
        if(IsConnect&&client.Connected)
        {
            Receive();//tcp연결은 얼만큼 데이터가 오갔는지를 세는 것으로 연결을 가늠할 수 있다.
            Send();
        }

        waittingCount++;
        if(waittingCount >=1000)
        {
            waittingCount = 0;
            typeBuff.Push(new DummyData());
        }
        return IsConnect;
    }//Send()와 Receive() 호출. 이 함수를 계속 호출해야 입출력이 이뤄짐.
    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    void Send()
    {
        int count=0;
        while (typeBuff.BinaryPull(out byte[] data))
        {
            count= client.Send(binaryHandler.Pack(data),SocketFlags.None,out SocketError error);
            if (error != SocketError.Success)
            {
                Console.WriteLine("실패. 소켓종료.");
                client.Close();
                IsConnect = false;
            }
            if(count==0)
            {
                IsConnect= false;
                client.Close();
            }
        }
    }//typeBuff.BinaryPull(out byte[] data)해서 뽑은 데이터를 모조리 송신
    void Receive()
    {
        int count = 0;
        while (client.Available>0)
        {
            byte[] buff = new byte[client.Available];
            count+= client.Receive(buff, 0,buff.Length, SocketFlags.None,out SocketError error);
            if (error != SocketError.Success)
            {
                Console.WriteLine("실패. 소켓종료.");
                client.Close();
                IsConnect = false;
                return;
            }
            foreach (byte b in buff)
            {
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1바이트씩 보내면 슬라이스될 때 true와 함꼐 out.
                {
                    typeBuff.BinaryPush(binarySplited);//핵심
                }
            }
        }
        return;
    }//typeBuff.BinaryPush(데이터)하여 수신한 데이터를 모조리 저장
}
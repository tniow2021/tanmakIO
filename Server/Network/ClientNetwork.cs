using System.Net.Sockets;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

public class ClientNetwork
{
    /*
     * 데이터 송신:
     * 1.아무 스크립트가 데이터를 Converting.To~~()함수를 통해 INetStruct형식의 구조체로 만든다.
     * 2.INetStruct 형식의 구조체를 Network.Send()함수에 보낸다
     * 3. Network.Send 함수는 구조체를 인코딩한 뒤 binaryHandler로 패킹한다.
     * 4. binaryHandler가 일정한 형식대로 패킹한 바이트열을 소켓으로 송신한다.
     * 
     * 데이터 수신:
     * 1. void ReceiveUpdate()가 돌면서 소켓으로 받는다
     * 2. binaryHandler객체는 데이터를 언팩하는데 데이터가 충분히 쌓여야 True를 반환한다.
     *    (이때 out키워드로 바이너리가 반환된다.)
     * 3. 바이트열을 typeBuff에 push()해준다. 
     * 4. typeBuff에 push된 데이터는 구조체로 디코딩되어 typeBuff내부 큐에 적재된다.
     * 5. 아무 스크립트나 typeBuff.pull(구조체의 타입코드)를 통해 원하는 구조체를 반환받을 수 있다 
     */
    //재연결및 면결예외관리는 나중에 구현해보자.

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
        //MonoBehaviour.print("연결성공");
    }
    public ClientNetwork(Socket _clinet, TypeBuff _typeBuff)
    {
        typeBuff = _typeBuff;
        client = _clinet;
        IsConnect = true;
    }
    int waittingCount = 0;//1000까지 세면 더미를 하나씩 보내어 상태를 체크한다.
    public bool Update()
    {
        if(IsConnect&&client.Connected)
        {
            Receive();//tcp연결은 얼만큼 데이터가 오갔는지를 세는 것으로 연결을 가늠할 수 있다.
            Send();
        }
        //Console.WriteLine("리시브버퍼:" + typeBuff.recieveQueues[0].Count);
        //Console.WriteLine("센드버퍼:" + typeBuff.SendQueues.Count);
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
                Console.WriteLine("실패. 소켓종료.");
                client.Close();
                IsConnect = false;
            }
            if(count==0)
            {
                IsConnect= false;
                client.Close();
            }
            Console.WriteLine("보낸데이터:" + count);
        }
    }
    float timeOutCount = 0;

    void Receive()
    {
        int count = 0;
        while (client.Available>0)
        {
            Console.WriteLine("완");
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
                Console.WriteLine(".." + b);
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1바이트씩 보내면 슬라이스될 때 true와 함꼐 out.
                {
                    typeBuff.BinaryPush(binarySplited);//핵심
                }
            }
        }
        return;
    }
}
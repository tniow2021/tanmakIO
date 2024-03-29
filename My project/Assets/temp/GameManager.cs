using System.Net;
using System;
using UnityEngine;
public class GameManager
{
    public static bool IsTest = true;
    /*
     * 
     */
    public static GameManager Instance { get; private set; }
    ClientNetwork network;
    public int ID { get; private set; } = 0;
    public static TypeBuff GetTypeBuff() { return Instance.network.typeBuff; }
    public static ClientNetwork GetNetwork() { return Instance.network; }
    public GameManager() 
    {
        if (Instance is not null) return;
        Instance = this;
        IPAddress ServerIP;
        if (IsTest)
        {
            ServerIP = IPAddress.Loopback;
        }
        else
        {
            ServerIP = Dns.GetHostAddresses("www.tempdomain123.shop")[0];
        }
        
        int port = 20240;
        network = new ClientNetwork(ServerIP, port,new TypeBuff());
        if (network.ConnectStart(second: 5))
        {
            MonoBehaviour.print("연결성공");
        }
        else
        {
            MonoBehaviour.print("연결실패");
        }
        GetTypeBuff().Push(new DummyData());//처음에 주는 패킷은 서버가 못받는 것 같아서 더미를 하나 보낸다.
        GetTypeBuff().Push(new AccessRequest());
    }
    public void Update()
    {
        while (GameManager.GetTypeBuff().pull(out INetStruct st, TypeCode.AccessRequestAnswer))
        {
            var aa = (AccessRequestAnswer)st;
            ID= aa.YourID;
            MonoBehaviour.print("ID등록받음:"+ ID);
        }
        network.Update();
    }
}

using System.Net;
using System;
using UnityEngine;
public class GameManager
{
    /*
     * 
     */
    public static GameManager Instance { get; private set; }
    ClientNetwork network;
    public int ID { get; private set; } = 0;
    public static TypeBuff GetTypeBuff() { return Instance.network.typeBuff; }
    public GameManager() 
    {
        if (Instance is not null) return;
        Instance = this;
        IPAddress ServerIP = Dns.GetHostAddresses("ec2-18-191-167-50.us-east-2.compute.amazonaws.com")[0];
        //IPAddress ServerIP = IPAddress.Parse(UIScript.IPv4);
        int port = 20240;
        //네트워크 객체에 타입버퍼 객체를 넣어준다.

        MonoBehaviour.print("연결성공");
        network = new ClientNetwork(ServerIP, port,new TypeBuff());
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

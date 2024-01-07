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
        //��Ʈ��ũ ��ü�� Ÿ�Թ��� ��ü�� �־��ش�.

        MonoBehaviour.print("���Ἲ��");
        network = new ClientNetwork(ServerIP, port,new TypeBuff());
        GetTypeBuff().Push(new DummyData());//ó���� �ִ� ��Ŷ�� ������ ���޴� �� ���Ƽ� ���̸� �ϳ� ������.
        GetTypeBuff().Push(new AccessRequest());
    }
    public void Update()
    {
        while (GameManager.GetTypeBuff().pull(out INetStruct st, TypeCode.AccessRequestAnswer))
        {
            var aa = (AccessRequestAnswer)st;
            ID= aa.YourID;
            MonoBehaviour.print("ID��Ϲ���:"+ ID);
        }
        network.Update();
    }
}

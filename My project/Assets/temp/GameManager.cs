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
            MonoBehaviour.print("���Ἲ��");
        }
        else
        {
            MonoBehaviour.print("�������");
        }
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

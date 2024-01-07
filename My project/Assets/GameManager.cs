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
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        //��Ʈ��ũ ��ü�� Ÿ�Թ��� ��ü�� �־��ش�.
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

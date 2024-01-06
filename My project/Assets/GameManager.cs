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
    public static TypeBuff GetTypeBuff() { return Instance.network.typeBuff; }
    public GameManager() 
    {
        if (Instance is not null) return;
        Instance = this;
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        //��Ʈ��ũ ��ü�� Ÿ�Թ��� ��ü�� �־��ش�.
        network = new ClientNetwork(ServerIP, port);
    }
    public void Update()
    {
        //MonoBehaviour.print("��:"+GetTypeBuff().recieveQueues[(Int32)TypeCode.UserTransform].Count+"��:"+ GetTypeBuff().SendQueues.Count);
        network.Update();
    }
}

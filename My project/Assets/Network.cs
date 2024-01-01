using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Network : MonoBehaviour
{
    //�ϴ� �������� �������� ����: �÷��̾��� ��ǥ��.
    //�̴� �÷��̾ �ʿ� ������ ���� ����̴�. 
    Socket client = 
        new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    IPEndPoint ServerIP = new IPEndPoint(IPAddress.Loopback, 2024);

    List<TempPlayer> players=new List<TempPlayer>();
    public void AccessRequest(TempPlayer player)
    {
        players.Add(player);
    }
    void Start()
    {
        client.Connect(ServerIP);
        print("���� ���Ἲ��");
    }
    void Update()
    {
        foreach(var player in players)
        {
            SendTransform(player);
        }
        print(players.Count);
    }
    void SendTransform(TempPlayer player)
    {
        Vector3 XYZ = player.transform.position;
        string msg = player.UserName +": "+ XYZ.ToString()+"\n";
        byte[]data= Encoding.UTF8.GetBytes(msg);
        client.Send(data);
    }
}

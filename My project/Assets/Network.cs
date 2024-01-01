using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Network : MonoBehaviour
{
    //일단 실험으로 보내야할 정보: 플레이어의 좌표값.
    //이는 플레이어가 맵에 존재할 때의 얘기이다. 
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
        print("서버 연결성공");
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

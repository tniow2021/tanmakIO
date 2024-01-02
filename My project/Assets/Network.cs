using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Network
{
    //일단 실험으로 보내야할 정보: 플레이어의 좌표값.
    //이는 플레이어가 맵에 존재할 때의 얘기이다. 

    //재연결및 면결예외관리
    Socket client;
    IPEndPoint ServerIP;
    int port;
    public Network(IPAddress connectAddress,int _port)
    {
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port= _port;

        //Start
        client.Connect(ServerIP);
        MonoBehaviour.print("서버 연결성공");
    }
    //(데이터를 분류하여, UI,로그인,게임등 여러가지로 받음)


    
}

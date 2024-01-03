using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Network network { get; private set; }
    public static  Network GetNetwork() { return Instance.network; }
    public GameManager() 
    {
        if (Instance is not null) return;
        Instance = this;
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        //네트워크 객체에 타입버퍼 객체를 넣어준다.
        network= new Network(ServerIP, port,new TypeBuff());
    }
}

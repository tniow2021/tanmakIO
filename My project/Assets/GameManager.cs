using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Network network { get; private set; }
    public GameManager() 
    {
        print("스태틱");
        if (Instance != null)
        {
            디버깅.게임매니저인스턴스가두개이상생성됨();
            return;
        }
        Instance = this;
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        network= new Network(ServerIP, port);

        
    }
}

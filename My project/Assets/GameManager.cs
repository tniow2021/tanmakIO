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
        print("����ƽ");
        if (Instance != null)
        {
            �����.���ӸŴ����ν��Ͻ����ΰ��̻������();
            return;
        }
        Instance = this;
        IPAddress ServerIP = IPAddress.Loopback;
        int port = 2024;
        network= new Network(ServerIP, port);

        
    }
}

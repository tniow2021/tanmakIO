using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class Network
{
    //�ϴ� �������� �������� ����: �÷��̾��� ��ǥ��.
    //�̴� �÷��̾ �ʿ� ������ ���� ����̴�. 

    //�翬��� ��Ό�ܰ���
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
        MonoBehaviour.print("���� ���Ἲ��");
    }
    //(�����͸� �з��Ͽ�, UI,�α���,���ӵ� ���������� ����)


    
}

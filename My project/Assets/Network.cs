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
    public void Send(byte[] data)
    {
        client.Send(data);
    }
    byte[] receiveBuff = new byte[8196];
    public string Recieve()//(����)
    {
        int n=client.Receive(receiveBuff);
        string susin=Encoding.Default.GetString(receiveBuff, 0, n);
        return susin;
    }
    //(�����͸� �з��Ͽ�, UI,�α���,���ӵ� ���������� ����)


    
}

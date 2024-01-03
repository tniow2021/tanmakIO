using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System;
using static TypeBuff;
using System.Linq;

public class Network
{
    //�ϴ� �������� �������� ����: �÷��̾��� ��ǥ��.
    //�̴� �÷��̾ �ʿ� ������ ���� ����̴�. 

    //�翬��� ��Ό�ܰ���
    Socket client;
    IPEndPoint ServerIP;
    int port;

    TypeBuff typeBuff;
    public Network(IPAddress connectAddress, int _port,TypeBuff tb)
    {
        typeBuff = tb;
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port = _port;
        
        //Start
        client.Connect(ServerIP);
        MonoBehaviour.print("���� ���Ἲ��");
    }
    //(�����͸� �з��Ͽ�, UI,�α���,���ӵ� ���������� ����)
    public void Send(TypeBuff.UserTransform ut)
    {
        SendData(ut.Encoding());
    }
    //���� ����:public void Receive(out InGameNet.UserTransform ut)


    void SendData(byte[]data)
    {

    }
    void ReceiveData()
    {
        
    }

    byte[] JoinArray(byte[] a, byte[]b)
    {
        byte[] c=new byte[a.Length+b.Length];
        int i = 0;
        for(i=0;i<a.Length;i++)
        {
            c[i] = a[i];
        }
        for(i=a.Length;i< a.Length+b.Length;i++)
        {
            c[i] = b[i-a.Length];
        }
        return c;
    }
}
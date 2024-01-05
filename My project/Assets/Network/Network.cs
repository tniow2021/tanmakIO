using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class Network
{
    /*
     * ������ �۽�:
     * 1.�ƹ� ��ũ��Ʈ�� �����͸� Converting.To~~()�Լ��� ���� INetStruct������ ����ü�� �����.
     * 2.INetStruct ������ ����ü�� Network.Send()�Լ��� ������
     * 3. Network.Send �Լ��� ����ü�� ���ڵ��� �� binaryHandler�� ��ŷ�Ѵ�.
     * 4. binaryHandler�� ������ ���Ĵ�� ��ŷ�� ����Ʈ���� �������� �۽��Ѵ�.
     * 
     * ������ ����:
     * 1. void ReceiveUpdate()�� ���鼭 �������� �޴´�
     * 2. binaryHandler��ü�� �����͸� �����ϴµ� �����Ͱ� ����� �׿��� True�� ��ȯ�Ѵ�.
     *    (�̶� outŰ����� ���̳ʸ��� ��ȯ�ȴ�.)
     * 3. ����Ʈ���� typeBuff�� push()���ش�. 
     * 4. typeBuff�� push�� �����ʹ� ����ü�� ���ڵ��Ǿ� typeBuff���� ť�� ����ȴ�.
     * 5. �ƹ� ��ũ��Ʈ�� typeBuff.pull(����ü�� Ÿ���ڵ�)�� ���� ���ϴ� ����ü�� ��ȯ���� �� �ִ� 
     */
    //�翬��� ��Ό�ܰ����� ���߿� �����غ���.

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


    BinaryHandler binaryHandler = new BinaryHandler(cutTrigger: 4);
    void Send()
    {
        if(typeBuff.BinaryPull(out byte[] data))
        {

            MonoBehaviour.print(13);
            client.Send(binaryHandler.Pack(data));
        }
    }
    public void Update()
    {
        Receive();
        Send();
    }
    void Receive()
    {
        MonoBehaviour.print("tlqkf");
        if (client.Available>0)
        {
            MonoBehaviour.print(-1);

            byte[] buff = new byte[client.Available];
            client.Receive(buff, client.Available,SocketFlags.None);
            foreach(byte b in buff)
            {
                if(binaryHandler.UnPack(b,out byte[]binarySplited))//1����Ʈ�� ������ �����̽��� �� true�� �Բ� out.
                {
                    MonoBehaviour.print(-2);
                    typeBuff.BinaryPush(binarySplited);//�ٽ�
                }
            }
        }
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
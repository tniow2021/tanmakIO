using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System;
using static TypeBuff;
using System.Linq;
using System.Runtime.InteropServices;

public class Network
{
    //일단 실험으로 보내야할 정보: 플레이어의 좌표값.
    //이는 플레이어가 맵에 존재할 때의 얘기이다. 

    //재연결및 면결예외관리

    Socket client;
    IPEndPoint ServerIP;
    int port;

    TypeBuff typeBuff;
    Queue<byte[]>structBinary_Queue=new Queue<byte[]>();
    public Network(IPAddress connectAddress, int _port,TypeBuff tb)
    {
        typeBuff = tb;
        client =
            new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ServerIP = new IPEndPoint(connectAddress, _port);
        port = _port;
        
        //Start
        client.Connect(ServerIP);
        MonoBehaviour.print("서버 연결성공");
    }    
    
    //구분자+다음구분자까지의 길이
    //만약 
    public void Send(INetStruct ns)
    {
        int structSize= Marshal.SizeOf(ns);
        byte[]sizeData=BitConverter.GetBytes(structSize);
        byte[] lastData = JoinArray(sizeData, ns.Encoding());

        client.Send(lastData);
    }
    public (bool success, byte[] structBinary) Receive()
    {
        if(structBinary_Queue.Count>0)
        {
            byte[] binary = structBinary_Queue.Dequeue();
            return (true, binary);
        }
        else { return (false,null); }
    }


    bool readChanger = false;
    byte 구분자 = 1;
    List<byte> byteList = new List<byte>();
    public void Update()
    {
        if(client.Available>0)
        {
            byte[] buff = new byte[client.Available];
            client.Receive(buff, client.Available,SocketFlags.None);
            foreach(byte b in buff)
            {
                if (b == 구분자)
                {
                    if (readChanger is false) readChanger = true;
                    else readChanger = false;
                }
                if(readChanger is true)//readChanger가 true면 struct데이터임
                {//사이즞즈즞ㅈ즈즈ㅡㅈ
                    byteList.Add(b);
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
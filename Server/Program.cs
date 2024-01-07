using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

//https://siku314.tistory.com/75
//https://sanghun219.tistory.com/104
static class Program
{
    static void Main()
    {

        myServer();
    }
    static void myServer()
    {
        Console.WriteLine("프로그램 시작");
        Server server = new Server(_port: 20240, _maxUser: 1000);
        while (true)
        {
            int onlineUserCount = server.Update();
            if (onlineUserCount <= 0)
            {
                Thread.Sleep(100);
            }
        }
    }
}
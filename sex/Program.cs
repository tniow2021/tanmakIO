
using sex;
using sex.Networking;
using System.Net.Sockets;

public static class Program
{
    static Root root;
    static int Main()
    {
        root=new Root();
        Test.AllTest();
        //Accepter accepter = new Accepter(2024, socketConnectEvent);
        Thread.Sleep(200000000);
        return 0;
    }
}
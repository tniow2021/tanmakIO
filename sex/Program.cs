
using sex;
using sex.Conversion;
using sex.GameLogic;
using sex.Networking;
using System.Net.Sockets;

public static class Program
{
    static Root root;
    static int Main()
    {
        root=new Root();
        Test.AllTest();
        Accepter accepter = new Accepter(2024, socketConnectEvent);

        Thread.Sleep(200000000);
        return 0;
    }
    static void socketConnectEvent(Socket socket)
    {
        UserIO userIO = new UserIO(packetSizeLimit: 1024);
        NetPacketDivider divider=new NetPacketDivider(root.NetPacketMinimumLengthTable,())
        userIO.SetUserIO(socket, 1014,()=>{)
    }
}


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

        testRoom = new Room(root.table);

        Thread.Sleep(200000000);
        return 0;
    }
    static Room testRoom;
    static void socketConnectEvent(Socket socket)
    {
        UserIO userIO = new UserIO(packetSizeLimit:1024);
        EnDecoder enDecoder=new EnDecoder(root.table,)//시발 
        userIO.SetUserIO(socket,1014,)
    }
}

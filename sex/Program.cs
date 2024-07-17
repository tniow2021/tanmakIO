
using sex;
using sex.Conversion;
using sex.GameLogic;
using sex.NetPackets;
using sex.Networking;

public static class Program
{
    static Root root;
    static FrontRoom front;
    static Program()
    {
        root = new Root();
        front = new FrontRoom();
    }
    static int Main()
    {
        sex.Setting.AllSetting();

        Accepter accepter = new Accepter(2024, front.Welcome);
        accepter.Start();
        Test.AllTest();

        Thread.Sleep(200000000);
        return 0;
    }
}

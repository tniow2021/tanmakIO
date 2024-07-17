using sex.DataStructure;
using sex.NetPackets;
using sex.Networking;
using System;
using System.Net.Sockets;

namespace sex.GameLogic
{
    public class User
    {
        public User(Socket sk)
        {
            UserIO userIO = Root.root.UserIOPool.GetBlock();
            NetPacketPort divider =
                new NetPacketPort(Root.root.NetPacketMinimumLengthTable, null);
            userIO.SetUserIO(sk, 1024, divider.Decode);
            userIO.recieveEvent = divider.Decode;
            this.divider = divider;
            this.userIO = userIO;
            this.localID = -1;
        }
        public void RegisterRecieveEvent(TakeDividedData func)
        {
            divider.SetTakeEvent(func);
        }
        public unsafe void Send<T>(ref T netPacket)where T : INetPacket
        {
            divider.Encode(userIO.sendBuff, netPacket);
            userIO.Send();
        }
        public UserIO userIO;
        public NetPacketPort divider;
        public int localID;
    }
}

using sex.Conversion;
using sex.DataStructure;
using sex.Networking;
using sex.NetPackets;
using sex.NetPacket;
namespace sex.GameLogic
{
    public class Room
    {
        //ConvertibleGroup group;

        List<User>users=new List<User>();//나중에 풀로 대체
        public Room()
        {
            //group = new ConvertibleGroup(Root.root.table);
        }
        //int useri = 0;
        //public void addUser(UserIO userIO)
        //{
        //    //아이디는 어떡해야 좋노...
        //    NetPacketDivider divider = 
        //        new NetPacketDivider(Root.root.NetPacketMinimumLengthTable, Process);
        //    userIO.recieveEvent = new TakeRecievedData((Span<byte> span) =>
        //    {
        //        return divider.Decode(span);
        //    }
        //    );
        //    User user = new User();
        //    user.divider = divider;
        //    user.userIO = userIO;
        //    users[useri] = user;
        //}
        public void AddUser(User user)
        {
            user.divider.SetTakeEvent(Process);
            users.Add(user);
        }
        void Process(int typeNumber, Span<byte> span, int offset)
        {
            switch (typeNumber)
            {
                case (int)EnumNetPacket.Vector3Int:
                    Vector3Int vector3Int = new Vector3Int(span,ref offset);
                    break;
            }
        }
    }
}
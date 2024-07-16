using sex.Networking;

namespace sex.GameLogic
{
    public class FrontRoom
    {
        Room 임시 = new Room();
        public void Welcome(UserIO userIO)
        {
            NetPacketDivider divider =
                new NetPacketDivider(Root.root.NetPacketMinimumLengthTable, null);
            userIO.recieveEvent = new TakeRecievedData((Span<byte> span) =>
            {
                return divider.Decode(span);
            }
            );
            User user = new User();
            user.divider = divider;
            user.userIO = userIO;

            임시.AddUser(user);
            user.userIO.ReciveStart();
        }
    }
}

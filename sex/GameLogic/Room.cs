using sex.NetPackets;
namespace sex.GameLogic
{
    public class Room
    {
        List<User> users = new List<User>();//나중에 풀로 대체
        public Room()
        {
        }
        public void AddUser(User user)
        {
            user.localID = 1;//임시
            user.divider.value = user.localID;
            user.RegisterRecieveEvent(Process);
            users.Add(user);
        }
        public void Process(int localUSerID, int typeNumber, Span<byte> span, int offset)
        {
            switch (typeNumber)
            {
                case (int)EnumNetPacket.Vector3Int:
                    Vector3Int v3 = new Vector3Int(span, ref offset);

                    Console.WriteLine($"id: {localUSerID}, {v3.x}, {v3.y}, {v3.z}");
                    break;
                default:
                    break;
            }
        }
    }
}
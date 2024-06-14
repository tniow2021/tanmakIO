using sex.Conversion;
using sex.DataStructure;
using sex.Networking;
using sex.UserDefinedNetPacket;
namespace sex.GameLogic
{
    public class Room
    {
        ConvertibleGroup group;
        User[] users;
        public Room()
        {
            group = new ConvertibleGroup(Root.root.table);
            users = new User[10];
            EnDecoder enDecoder = new EnDecoder(group,
                (INetConvertible data) => {
                    Vecter3Int mydata = (Vecter3Int)data;
                    group.ReturnBlock(data);
                }
            );
        }
        int useri = 0;
        public void addUser(UserIO userIO)
        {
            //아이디는 어떡해야 좋노...
            var enDecoder=new EnDecoder(group, (INetConvertible s) => { Process(id:1,s); });
            userIO.recieveEvent = new TakeRecievedData((Span<byte>span) =>
            {
                return enDecoder.Decode(span);
            }
            );
            User user=new User();
            user.enDecoder=enDecoder;
            user.userIO=userIO;
            users[useri] = user;
        }
        void Process(int id,INetConvertible data)
        {
            switch(data.GetTypeNumber())
            {

            }
        }
    }
}
public struct User
{
    public UserIO userIO;
    public EnDecoder enDecoder;
}
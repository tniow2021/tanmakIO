using System.Net.Sockets;

namespace sex.GameLogic
{
    public class FrontRoom
    {
        Room 임시 = new Room();
        public void Welcome(Socket socket)
        {
            User user = new User(socket);

            임시.AddUser(user);
            user.userIO.ReciveStart();
        }
    }
}

﻿using System.Net.Sockets;

namespace sex.GameLogic
{
    public class FrontRoom
    {
        Room 임시 = new Room();
        public void Welcome(Socket socket)
        {
            Console.WriteLine("새 유저 접속");
            User user = new User(socket);
            임시.AddUser(user);
            user.userIO.ReciveStart();
        }
    }
}

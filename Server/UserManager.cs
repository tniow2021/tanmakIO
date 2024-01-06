using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserManager
{
    int UserIdCount = 1;
    Queue<int>IDsOfUserWhoLeft=new Queue<int>();

    List<User>Users=new List<User>();
    public int CreateUser(ClientNetwork network)
    {
        int newID = CreateUserID();
        Users.Add(new User(newID, network));
        return newID;
    }
    int CreateUserID()
    {
        if(IDsOfUserWhoLeft.Count>0)
        {
            return IDsOfUserWhoLeft.Dequeue();
        }
        else
        {
            int a = UserIdCount;
            UserIdCount++;
            return a;
        }
    }
}
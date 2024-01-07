public class UserManager
{
    int UserIdCount = 1;
    List<int>IDsOfUserWhoLeft=new List<int>();

    Dictionary<ClientNetwork, User>Users=new Dictionary<ClientNetwork, User>();
    public int CreateUser(ClientNetwork network)
    {
        int newID = CreateUserID();
        Users.Add(network, new User(newID, network));
        return newID;
    }
    public void RemoveUser(ClientNetwork network)
    {
        Users.Remove(network);
        IDsOfUserWhoLeft.Add(Users[network].ID);
    }
    public void RemoveUser(User user)
    {
        Users.Remove(user.network);
    }
    public bool GetUser(ClientNetwork network, out User user)
    {
        if(Users.ContainsKey(network))
        {
            user = Users[network];
            return true;
        }
        user = null;
        return false;
    }
    int CreateUserID()
    {
        if(IDsOfUserWhoLeft.Count>0)
        {
            int ID=IDsOfUserWhoLeft[IDsOfUserWhoLeft.Count-1];
            IDsOfUserWhoLeft.Remove(ID);
            return ID;
        }
        else
        {
            int a = UserIdCount;
            UserIdCount++;
            return a;
        }
    }
}
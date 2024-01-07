public class User
{
    public int ID { get; private set; }
    public ClientNetwork network { get; private set; }
    public User(int _ID, ClientNetwork _network)
    {
        ID = _ID;
        network = _network;
    }
}
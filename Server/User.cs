using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
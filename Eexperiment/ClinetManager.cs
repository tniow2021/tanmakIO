using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Eexperiment
{
    public class ClinetManager
    {
        List<Socket>clients=new List<Socket>();
        Queue <Socket> temp_Waiting_Line=new Queue<Socket> ();
        public void RegisterClient(Socket _client)
        {
            temp_Waiting_Line.Enqueue(_client);
        }
        public void DeleteClinet(Socket _client)
        {

        }
        public List<Socket> GetClinets()
        {
            if(temp_Waiting_Line.Count>0)
            {
                lock (temp_Waiting_Line)
                {
                    foreach(var s in temp_Waiting_Line)
                    {
                        clients.Add(s);
                    }
                }
            }
            return clients;
        }
    }
}

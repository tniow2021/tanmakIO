using Conversion;
using System.Net;
using System.Net.Sockets;

public class Clinet
{
    public Socket sk;
    public int UserID = 0;
    public Clinet(Socket _sk)
    {
        sk = _sk;
    }
}
delegate void TakeData(Convertible cb);
class Harbor: IWork
{
    TakeData[] takeData;
    int maxPortIndex;
    Socket listener;
    List<Clinet>clinets = new List<Clinet>();
    public Harbor(short portNumber)
    {
        takeData = new TakeData[portNumber];
        maxPortIndex = portNumber - 1;

        listener=new Socket(
            AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, portNumber));
        listener.Listen(1000);
    }
    public bool RegisterPort(int port, TakeData TD)//중복등록은 안되게함
    {
        if (takeData[port] is not null)
        {
            if (port > maxPortIndex) return false;

            takeData[port] = TD;
            return true;
        }
        else return false;
    }
    public int Update()
    {
        try
        {
            Console.WriteLine("접속받는중....");
            Socket clinetSock= listener.Accept();
            clinets.Add(new Clinet(clinetSock));
            Console.WriteLine($"접속함 현재{clinets.Count}명.");
        }
        catch (SocketException e)
        {
            Console.WriteLine (e.ToString());
        }
        finally
        {

        }
        return 0;
    }
    public int Send(int UserID,Span<byte> data)
    {
        int count=0;
        try
        {
            Clinet cn = clinets.Find(n => n.UserID == UserID);
            count =cn.sk.Send(new byte[2] {2,3},SocketFlags.None,out SocketError se);
            if(se != SocketError.Success)
            {
                HappenSocketError(cn, se);
            }
        }
        catch
        {
            count = 0;
            Console.WriteLine($"d카운트{count}");
        }
        finally
        {
            
        }
        return count;
    }
    void HappenSocketError(Clinet cn, SocketError se)
    {

        if (se != SocketError.Success)
        {
            clinets.Remove(cn);
            cn.sk.Close();
        }
        /*
        switch(se)
        {
            case SocketError.NetworkReset:
                clinets.Remove(sk);
                sk.Close();
                break;
        }
        */
    }
}

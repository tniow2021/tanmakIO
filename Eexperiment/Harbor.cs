using Conversion;
using Eexperiment;
using System.Net;
using System.Net.Sockets;
using static System.Console;
public class Clinet
{
    public Socket sk;
    public int UserID = 0;
    public Clinet(Socket _sk)
    {
        sk = _sk;
    }
    public bool Recieve()
    {
        return true;
    }
    public bool Send()
    {
        return true;
    }
    public bool AliveCherk()
    {
        return true;
    }
}
delegate void TakeData(Convertible cb);
class Harbor
{
    TakeData[] takeData;
    int maxPortIndex;
    Socket listener;
    public Harbor(short portNumber)
    {
        takeData = new TakeData[portNumber];
        maxPortIndex = portNumber - 1;

        listener=new Socket(
            AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(new IPEndPoint(IPAddress.Any, portNumber));
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

    bool stopAcceping = false;
    public IsSuccess AcceptingStart()
    {
        try
        {
            stopAcceping = false;
            listener.Listen(1000);
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(비동기적완료);
            Accepting(args);
            return IsSuccess.Success;
        }
        catch(Exception e)
        {
            WriteLine(e.ToString());
            return IsSuccess.failure;
        }
    }
    public void AcceptingFinish()
    {
        stopAcceping=true;
        //여기에 비동기 작업을 취소하는 코드추가
    }
    void Accepting(SocketAsyncEventArgs args)
    {
        args.AcceptSocket = null;
        bool willRaiseEvent = false;
        while (!willRaiseEvent)//동기적으로 가능한 처리(=밀려있는 연결)를 다 하기
        {
            if (stopAcceping is true) return;
            // 서버는 승인요청을 받는 작업을 할 SocketAsyncEventArgs 객체와 함께
            // 클라이언트의 요청이 들어오면 승인 작업을 하도록 처리합니다.
            bool pending = listener.AcceptAsync(args);
            //비동기로 승인을 대기해야할 때가 오면 pending이 true가 되고 루프가 끝난다.
            //args에 넣어둔 콜백함수는 이 함수를 다시 호출한다. 
            if (pending == false)
            {
                ProcessAccept(args);
            }
        }
    }
    void 비동기적완료(object sender,SocketAsyncEventArgs args)
    {
        ProcessAccept(args);
        //반복
        Accepting(args);
    }
    void ProcessAccept(SocketAsyncEventArgs args)
    {

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
/*
 * 많은 유저에게서 데이터를 받는 일은 병렬처리가 필요한 작업이다.
 * 하나의 작업에서 유저들을 모두 살피며 데이터를 받아도 되지만 병렬화하는 게 좋다.
 * 유저하나 당 task를 만들면 어차피 쓰레들 풀과 스케줄러에서 차곡차곡 대기당하고 
 *  오버헤드가 큼으로 하지말아야한다. 
 * 대신 사용자 기준에서 끊기지 않을정도의 단위로 묶어서 대략 수백명당 하나의 task를
 *  만들도록 한다. 
 * 단 그 task의 숫자가 cpu쓰레드의 수를 넘어가지 않도록한다. 어차피 쓰레드풀에서 돌아감으로.
 *  (task를 많이 만들 수록 성능이 올라가는진 모르겠다. 아마 아닐 것이다.);
 */
using System.Net.Sockets;
using System.Net;
namespace sex.Networking
{
    public class Accepter : Machine
    {
        Socket listener;
        Action<Socket> socketConnectEvent;
        SocketAsyncEventArgs args;
        public Accepter(ushort portNumber, Action<Socket> socketConnectEvent)
        {
            listener = new Socket(
                AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, portNumber));
            this.socketConnectEvent = socketConnectEvent;


            args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(CallbackOfArgs);
        }

        bool stop = true;

        public IsSuccess Start()
        {
            try
            {
                if (stop == true)
                {
                    stop = false;
                    listener.Listen(1000);
                    Accepting(args);
                    return IsSuccess.Success;
                }
                else return IsSuccess.failure;

            }
            catch (Exception e)
            {
                return IsSuccess.failure;
            }
        }
        public void Stop()
        {
            stop = true;
            //&&&여기에 비동기 작업을 취소하는 코드추가
        }
        void Accepting(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            bool pending = false;
            while (!pending)//동기적으로 가능한 처리(=밀려있는 연결)를 다 하기
            {
                if (stop is true) return;
                // 서버는 승인요청을 받는 작업을 할 SocketAsyncEventArgs 객체와 함께
                // 클라이언트의 요청이 들어오면 승인 작업을 하도록 처리합니다.

                //WriteLine("씨발1");
                pending = listener.AcceptAsync(args);
                //비동기로 승인을 대기해야할 때가 오면 pending이 true가 되고 루프가 끝난다.
                //args에 넣어둔 콜백함수는 이 함수를 다시 호출한다. 
                if (pending == false)
                {
                    ProcessAccept(args);
                }
            }
        }
        void CallbackOfArgs(object? sender, SocketAsyncEventArgs args)
        {
            ProcessAccept(args);
            //반복
            Accepting(args);
        }
        void ProcessAccept(SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                socketConnectEvent(args.AcceptSocket);
            }
        }
    }
}

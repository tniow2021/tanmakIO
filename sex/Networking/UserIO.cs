using System.Net.Sockets;

namespace sex.Networking
{
    public class UserIO: Machine
    {
        uint id;
        Socket sk;
        public UserIO(Socket _sk, uint _id)
        {
            id= _id;
            sk= _sk;
        }
        SocketAsyncEventArgs args;
        public void Init()
        {
            args=new SocketAsyncEventArgs();
            args.SetBuffer(new byte[1000]);//나중에 바이트 풀로 대체
            args.Completed += new EventHandler<SocketAsyncEventArgs>(DataRecieveEvent);
        }
        void DataRecieveEvent(object sender, SocketAsyncEventArgs a)
        {
            //1. 디코딩
            //2. 룸에 있는 큐에 적재 혹은 (개별적인 처리의 경우)메소드호출해서 처리
            //3. 다시 asyncRecive 호출
        }
        public IsSuccess Start()
        {
            bool pending = false;
            while(!pending)
            {
                pending =sk.ReceiveAsync(args);
                if (pending == false)
                {
                    DataRecieveEvent(null,args);
                }
            }
            return IsSuccess.Success;
        }
        public void Stop()
        {

        }
    }
}

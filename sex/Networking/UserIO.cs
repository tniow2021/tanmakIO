using sex.Pooling;
using System.Net.Sockets;
using sex.DataStructure;
namespace sex.Networking
{
    public class UserIO : PoolingObjects
    {
        static readonly Socket emptySock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static readonly Action<UserIOError> emptyEvent = (UserIOError u) => { };
        static readonly TakeRecievedData emptyEvent2 = (Span<byte> sp) => { return 0; };

        public Action<UserIOError> errorEvent = emptyEvent;
        public TakeRecievedData recieveEvent = emptyEvent2;
        Socket sk = emptySock;
        DynamicBuff<byte> recieveBuff;
        public DynamicBuff<byte> sendBuff { get; private set; }
        int packetSizeLimit;
        SocketAsyncEventArgs SocketArgs;

        public UserIO(int packetSizeLimit = 1024)
        {
            recieveBuff = new DynamicBuff<byte>();
            sendBuff = new DynamicBuff<byte>();
            SocketArgs = new SocketAsyncEventArgs();
            SocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Recieve_Completed);
            IsRunning = false;
        }
        public void SetUserIO(Socket sk, int packetSizeLimit, TakeRecievedData recieveEvent)
        {
            this.sk = sk;
            this.packetSizeLimit = packetSizeLimit;
            this.recieveEvent = recieveEvent;
        }

        // interface method of MultilayerPoolingObjects
        public void Assemble()
        {
            recieveBuff.SetBuff(Root.root.byte10000arrayPool.GetBlock(), 0, 0);
            sendBuff.SetBuff(Root.root.byte1000arrayPool.GetBlock(), 0, 0);
            SocketArgs.SetBuffer(recieveBuff.GetBuff(), 0, recieveBuff.GetNumContiguousSpaces());
        }
        public void Disassemble()
        {
            Root.root.byte10000arrayPool.RepayBlock(recieveBuff.GetBuff());
            Root.root.byte1000arrayPool.RepayBlock(sendBuff.GetBuff());
            recieveBuff.SetBuff(null);
            SocketArgs.SetBuffer(null);
        }

        //
        public bool IsRunning { get; private set; }
        bool stopSign = false;
        public void AllStop()
        {
            stopSign = true;
        }

        // unique method
        public void Send()//임시
        {
            sk.Send(sendBuff.GetBuff());
        }

        public IsSuccess ReciveStart()//이미 비동기함수가 실행중인데 두번 호출되면 안됨.
        {
            if (sk is null) return IsSuccess.failure;
            if (IsRunning is false)
            {
                //초기화 해야하나,,,
                int n = recieveBuff.GetNumContiguousSpaces();
                int w = recieveBuff.GetWriteOffset();
                SocketArgs.SetBuffer(recieveBuff.GetBuff(), w, n);
                RecieveProcess(SocketArgs);
                IsRunning = true;

                return IsSuccess.Success;
            }
            return IsSuccess.failure;
        }
        void Recieve_Completed(object? s, SocketAsyncEventArgs args)
        {
            Recieve_Data(args);
            RecieveProcess(args);
        }
        bool ArgsSetOffset(SocketAsyncEventArgs args)
        {
            int n = recieveBuff.GetNumContiguousSpaces();
            if (n >= packetSizeLimit + 5)//그냥 1만 더해도 되지만 넉넉하게 5
            {
                int w = recieveBuff.GetWriteOffset();
                //Console.WriteLine("남은  용량" + n);
                args.SetBuffer(w, n);
                return true;
            }
            else
            {
                recieveBuff.Arrange();
                n = recieveBuff.GetNumContiguousSpaces();
                int w = recieveBuff.GetWriteOffset();
                //Console.WriteLine("남은  용량" + n);
                if (n < packetSizeLimit + 5)
                {
                    return false;
                }
                args.SetBuffer(w, n);
                return true;
            }
        }
        void RecieveProcess(SocketAsyncEventArgs args)
        {
            bool pending = false;
            while (!pending)//즉시 완료시
            {
                bool IsSuccess = ArgsSetOffset(args);
                if (IsSuccess is false)
                {
                    stopSign = true;
                    errorEvent(UserIOError.InsufficientBufferSpace);
                    return;
                }
                //Console.WriteLine("비동기동작중");
                if (stopSign is true)
                    return;
                //Console.WriteLine("비동기동작중2");
                pending = sk.ReceiveAsync(args);
                if (pending is false && stopSign is false)
                {
                    Recieve_Data(args);
                }
            }
        }
        void Recieve_Data(SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                recieveBuff.IncreaseWriteOffset(args.BytesTransferred);
                if (recieveBuff.NonCountingRead(out Span<byte> span))
                {
                    int numberOfProcessedByte = recieveEvent(span);
                    recieveBuff.IncreaseReadOffset(numberOfProcessedByte);
                }
            }
            else
            {
                ErrorProcess(args.SocketError);
            }
        }
        void ErrorProcess(SocketError error)
        {
            switch (error)
            {
                case SocketError.SocketError:
                    stopSign = true;
                    //Console.WriteLine("닫힘");
                    sk.Close();
                    //Console.WriteLine("닫힘2");
                    errorEvent(UserIOError.SocketClose);
                    break;
                default:
                    stopSign = true;
                    //Console.WriteLine("닫힘");
                    sk.Close();
                    //Console.WriteLine("닫힘2");
                    errorEvent(UserIOError.SocketClose);
                    break;
            }
        }
    }
}

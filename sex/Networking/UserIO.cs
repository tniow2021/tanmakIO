using sex.Pooling;
using System.Net.Sockets;
using sex.DataStructure;
namespace sex.Networking
{
    public class UserIO : PoolingObjects
    {
        static readonly Socket emptySock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static readonly Action<UserIOError> emptyEvent = (UserIOError u) => { };

        public Action<UserIOError> errorEvent = emptyEvent;
        public TakeRecievedData recieveEvent;
        Socket sk = emptySock;
        DynamicBuff<byte> dynamicBuff;
        int packetSizeLimit;
        SocketAsyncEventArgs SocketArgs;
        
        public UserIO(int packetSizeLimit=1024)
        {
            dynamicBuff=new DynamicBuff<byte> ();
            SocketArgs = new SocketAsyncEventArgs();
            SocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Recieve_Completed);
            IsRunning = false;
        }
        public void SetUserIO(Socket sk,int packetSizeLimit, TakeRecievedData recieveEvent)
        {
            this.sk = sk;
            this.packetSizeLimit = packetSizeLimit;
            this.recieveEvent = recieveEvent;
        }

        // interface method of MultilayerPoolingObjects
        public void Assemble()
        {
            dynamicBuff.SetBuff(Root.root.byte10000arrayPool.GetBlock(),0,0);
            SocketArgs.SetBuffer(dynamicBuff.GetBuff(),0, dynamicBuff.GetNumContiguousSpaces());
        }
        public void Disassemble()
        {
            Root.root.byte10000arrayPool.RepayBlock(dynamicBuff.GetBuff());
            dynamicBuff.SetBuff(null);
            SocketArgs.SetBuffer(null);
        }

        //
        public bool IsRunning { get; private set; }
        bool stopSign = false;
        public void AllStop()
        {
            stopSign = false;
        }

        // unique method
        public IsSuccess ReciveStart()//이미 비동기함수가 실행중인데 두번 호출되면 안됨.
        {
            if (sk is null) return IsSuccess.failure;
            if(IsRunning is false)
            {
                //초기화 해야하나,,,
                int n = dynamicBuff.GetNumContiguousSpaces();
                int w = dynamicBuff.GetWriteOffset();
                SocketArgs.SetBuffer(dynamicBuff.GetBuff(), w, n);
                RecieveProcess(SocketArgs);
                IsRunning = true;
                return IsSuccess.Success;
            }
            return IsSuccess.failure;
        }
        void Recieve_Completed(object? s,SocketAsyncEventArgs args)
        {
            Recieve_Data(args);
            RecieveProcess(args);
        }
        bool ArgsSetOffset(SocketAsyncEventArgs args)
        {
            int n = dynamicBuff.GetNumContiguousSpaces();
            if (n>= packetSizeLimit+5)//그냥 1만 더해도 되지만 넉넉하게 5
            {
                int w = dynamicBuff.GetWriteOffset();
                //Console.WriteLine("남은  용량" + n);
                args.SetBuffer(w, n);
                return true;
            }
            else
            {
                dynamicBuff.Arrange();
                n=dynamicBuff.GetNumContiguousSpaces();
                int w = dynamicBuff.GetWriteOffset();
                //Console.WriteLine("남은  용량" + n);
                if(n< packetSizeLimit+5)
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
            while(!pending)//즉시 완료시
            {
                bool IsSuccess= ArgsSetOffset(args);
                if(IsSuccess is false)
                {
                    stopSign = true;
                    errorEvent(UserIOError.InsufficientBufferSpace);
                    return;
                }
                //Console.WriteLine("비동기동작중");
                if (stopSign is true)
                    return;
                //Console.WriteLine("비동기동작중2");
                pending =sk.ReceiveAsync(args);

                if(pending is false&& stopSign is false)
                {  
                    Recieve_Data(args);
                }
            }    
        }
        void Recieve_Data( SocketAsyncEventArgs args)
        {
            if(args.SocketError== SocketError.Success)
            {
                dynamicBuff.IncreaseWriteOffset(args.BytesTransferred);
                if(dynamicBuff.NonCountingRead(out Span<byte>span))
                {
                    int numberOfProcessedByte = recieveEvent(span);
                    dynamicBuff.IncreaseReadOffset(numberOfProcessedByte);
                }
            }
            else
            {
                ErrorProcess(args.SocketError);
            }
        }
        
        //public void Read()
        //{
        //    if(dynamicBuff.ReadAll(out Span<byte>data))
        //    {
        //        for(int i=0;i<data.Length;i++)
        //        {
        //            Console.Write(data[i]+" ");
        //        }
        //        Console.WriteLine();
        //    }
        //}
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

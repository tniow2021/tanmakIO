using sex.Pooling;
using System.Net.Sockets;
using sex.DataStructure;
using System.Text;
namespace sex.Networking
{
    public class UserIO : MultilayerPoolingObjects, Machine
    {
        static Socket emptySock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        DynamicBuff<byte> dynamicBuff;
        public Socket sk;
        int id;
        SocketAsyncEventArgs SocketArgs;
        public UserIO()//Socket sk, int id)
        {
            dynamicBuff=new DynamicBuff<byte> ();
            sk = emptySock;
            SocketArgs = new SocketAsyncEventArgs();
            SocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(Recieve_Completed);
            IsRunning = false;
        }
        public void SetUserIO(Socket sk, int id)
        {
            this.sk = sk;
            this.id = id;
        }

        // interface method of MultilayerPoolingObjects
        public void Assemble()
        {
            dynamicBuff.SetBuff(Root.root.byte10000arrayPool.GetBlock());
            SocketArgs.SetBuffer(dynamicBuff.GetBuff(),0, dynamicBuff.GetNumContiguousSpaces());
        }
        public void Disassemble()
        {
            Root.root.byte10000arrayPool.RepayBlock(dynamicBuff.GetBuff());
            dynamicBuff.SetBuff(null);
            SocketArgs.SetBuffer(null);
        }

        // interface method of Machine
        public bool IsRunning { get; private set; }
        bool stopSign = false;
        public IsSuccess Start()
        {
            IsRunning = true;
            return IsSuccess.Success;
        }
        public void Stop()
        {
            stopSign = false;
        }

        // unique method
        public IsSuccess ReciveStart()//이미 비동기함수가 실행중인데 두번 호출되면 안됨.
        {
            if (sk is null) return IsSuccess.failure;
            if(IsRunning is false)
            {
                RecieveProcess(SocketArgs);
                IsRunning = true;
                return IsSuccess.Success;
            }
            return IsSuccess.failure;
        }
        void RecieveProcess(SocketAsyncEventArgs args)
        {
            int n = dynamicBuff.GetNumContiguousSpaces();
            if (n>=20)
            {
                int w = dynamicBuff.GetWriteOffset();
                SocketArgs.SetBuffer(dynamicBuff.GetBuff(), w, n);
            }
            else
            {
                dynamicBuff.Arrange();
                n=dynamicBuff.GetNumContiguousSpaces();
                int w = dynamicBuff.GetWriteOffset();
                SocketArgs.SetBuffer(dynamicBuff.GetBuff(), w, n);
            }

            Console.WriteLine("흠..");
            bool pending = false;
            while(!pending)//즉시 완료시
            {
                pending=sk.ReceiveAsync(SocketArgs);
                if(!pending&& stopSign is false)
                {
                    Recieve_Completed(null,SocketArgs);
                }
            }    
        }
        void Recieve_Completed(object? s, SocketAsyncEventArgs args)
        {
            if (stopSign is true)
                return;

            switch (args.SocketError)
            {
                case SocketError.Success:
                    //
                    dynamicBuff.IncreaseWriteOffset(args.BytesTransferred);
                    //
                    Read();
                    Console.WriteLine("흠..2");
                    RecieveProcess(args);
                    break;
                case SocketError.SocketError:
                    stopSign = true;
                    break;
                default:
                    stopSign = true;
                    break;
            }
        }
        Decoder decoder = System.Text.Encoding.Default.GetDecoder();
        Memory<char>message=new Memory<char>(new char[2000]);
        public void Read()
        {
            if(dynamicBuff.ReadAll(out Span<byte>data))
            {
                decoder.Convert(
                    bytes: data,
                    chars: message.Span,
                    flush: false,
                    out int bytesUsed,out int charsUsed, out bool completed);
                    {
                    Console.WriteLine(message);
                }
            }
        }
    }
}

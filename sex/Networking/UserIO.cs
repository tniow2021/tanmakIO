using sex.Pooling;
using System.Net.Sockets;

namespace sex.Networking
{
    //public class UserIO: Machine
    //{
    //    uint id;
    //    Socket sk;
    //    public UserIO(Socket _sk, uint _id)
    //    {
    //        id= _id;
    //        sk= _sk;
    //    }
    //    SocketAsyncEventArgs args;
    //    public void Init()
    //    {
    //        args=new SocketAsyncEventArgs();
    //        args.SetBuffer(new byte[1000]);//나중에 바이트 풀로 대체
    //        args.Completed += new EventHandler<SocketAsyncEventArgs>(DataRecieveEvent);
    //    }
    //    void DataRecieveEvent(object sender, SocketAsyncEventArgs a)
    //    {
    //        //1. 디코딩
    //        //2. 룸에 있는 큐에 적재 혹은 (개별적인 처리의 경우)메소드호출해서 처리
    //        //3. 다시 asyncRecive 호출
    //    }
    //    public IsSuccess Start()
    //    {
    //        bool pending = false;
    //        while(!pending)
    //        {
    //            pending =sk.ReceiveAsync(args);
    //            if (pending == false)
    //            {
    //                DataRecieveEvent(null,args);
    //            }
    //        }
    //        return IsSuccess.Success;
    //    }
    //    public void Stop()
    //    {

    //    }
    //}

    public class UserIO : MultilayerPoolingObjects, Machine
    {
        static byte[] emptyArr = Array.Empty<byte>();
        static Socket emptySock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        byte[] ringBuff;
        public Socket sk;
        int id;
        SocketAsyncEventArgs SocketArgs;
        public UserIO()//Socket sk, int id)
        {
            ringBuff = emptyArr;
            sk = emptySock;
            SocketArgs = new SocketAsyncEventArgs();
            SocketArgs.Completed += new EventHandler<SocketAsyncEventArgs>(RecieveProcess);
            IsRunning = false;
        }

        // for ring buff system
        int buffLength;
        int writeIndex;
        int readIndex;
        int numBytesRemaining;
        void SetRingBuff(SocketAsyncEventArgs args,int numBytesRead)
        {
            writeIndex += numBytesRead;
            numBytesRemaining += numBytesRead;

            int count=0;
            if(readIndex <writeIndex)
            {
                count = buffLength - writeIndex;
            }
            else if(writeIndex<readIndex)
            {
                count = readIndex - writeIndex;
            }
            //둘이 같으면 writeIndex가 배열의 끝에 도착한것 .
            //설계상 writeIndex가 buffLength 보다 큰 경우는 없음.
            else if(writeIndex >= buffLength) {
                writeIndex = 0;
                count = buffLength;
            }



            SocketArgs.SetBuffer(
                offset: writeIndex,
                count: count
                );

        }


        // interface method of MultilayerPoolingObjects
        public void Assemble()
        {
            ringBuff = Root.root.byte10000arrayPool.GetBlock();

            buffLength = ringBuff.Length;
            writeIndex = 0;
            readIndex = 0;
            numBytesRemaining = 0;

            SetRingBuff(SocketArgs, 0);
        }
        public void Disassemble()
        {
            Root.root.byte10000arrayPool.RepayBlock(ringBuff);

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
        public void ReciveStart(SocketAsyncEventArgs args)
        {
            SocketArgs.SetBuffer(ringBuff);
            bool pending = false;
            while(!pending)//즉시 완료시
            {
                pending=sk.ReceiveAsync(SocketArgs);
                if(!pending)
                {
                    RecieveProcess(null,SocketArgs);
                }
            }    
        }
        void RecieveProcess(object s, SocketAsyncEventArgs args)
        {
            if (stopSign is true)
                return;

            switch (args.SocketError)
            {
                case SocketError.Success:
                    numBytesRemaining += args.BytesTransferred;
                    break;
                case SocketError.SocketError:
                    stopSign = true;
                    break;
                default:
                    stopSign = true;
                    break;
            }
        }
    }
}

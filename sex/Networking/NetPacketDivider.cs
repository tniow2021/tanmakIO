using sex.DataStructure;
using sex.NetPackets;
namespace sex.Networking
{
    public class NetPacketDivider
    {
        TakeDividedData take;
        const byte Identifier = 0b11111111;
        Table<int> lengthTable;
        public NetPacketDivider(Table<int> lengthTable, TakeDividedData take)
        {
            this.take = take;
            this.lengthTable = lengthTable;
        }

        //버퍼가 꽉차면 false
        public unsafe bool Encode(DynamicBuff<byte> buff, INetPacket c)
        {
            int n = 2 + 2 + 2 + c.GetLength();
            if (buff.Write(n, out Span<byte> span))
            {
                short typeNumber = c.GetTypeNumber();
                short size = c.GetLength();


                span[0] = 0b11111111;
                byte* bp = (byte*)&typeNumber;
                span[1] = *bp;
                span[2] = *(bp + 1);

                bp = (byte*)&size;
                span[3] = *bp;
                span[4] = *(bp + 1);

                int offset = 5;
                c.Encode(span, ref offset);
                span[offset] = 0b11111111;

                //for (int i = 0; i < span.Length; i++)
                //    Console.WriteLine(i + " :2: " + span[i]);
                return true;
            }
            return false;
        }
        enum ProcessState
        {
            IsIdentifier,
            IsTypeNumber,
            IsLengthByte,
            IsData,
            IsUnnown
        }
        TakeDividedData? temp=null;
        public void SetTakeEvent(TakeDividedData take)//이 함수는 한 쓰레드만 접근해야함.
        {
            temp = take;
        }
        ProcessState mode = ProcessState.IsUnnown;
        public unsafe int Decode(Span<byte> span)
        {
            //for (int i = 0; i < span.Length; i++)
            //    Console.WriteLine(i + " : " + span[i]);

            if (span.Length < 5)
                return 0;

            int nBytesProcessed = 0;

            short typeNumber = 0;
            short pactetLength = 0;
            while (true)
            {
                //1단계
                int offset = nBytesProcessed;
                if (span.Length - offset < 6)//패킷의 최소크기
                {
                    break;
                }
                if ((span[offset] == Identifier) is false)
                {
                    nBytesProcessed += 1;
                    continue;
                    //Identifier가 나올 때까지 무한 반복;
                }
                offset += 1;

                //2단계
                byte* t = (byte*)&typeNumber;
                *(t + 0) = span[offset];
                *(t + 1) = span[offset + 1];
                t = (byte*)&pactetLength;
                *(t + 0) = span[offset + 2];
                *(t + 1) = span[offset + 3];

                // Console.WriteLine("아이 씨발 섹스" + typeNumber);
                if (typeNumber < 0 || typeNumber > lengthTable.maxNumber)
                {
                    nBytesProcessed += 1;
                    continue;
                }
                //var convertible = convertibleGroup.GetBlock(typeNumber);
                int minimumLength = lengthTable.Get(typeNumber);
                offset += 4;

                //Console.WriteLine("start0"+ minimumLength+" "+ pactetLength);
                //세부적인 검사
                if (pactetLength < minimumLength)
                {
                    //이 경우는 클라이언트가 잘못 보냈거나
                    //아주 작은 확률로  startIdentifier 부터 잘못된 것.
                    nBytesProcessed += 1;
                    continue;
                }
                //Console.WriteLine("start1");
                if (span.Length - (offset + pactetLength) < 0)//패킷이 다 안들어 있으면 
                {
                    break;
                }
                // Console.WriteLine("start2");
                if ((span[offset + pactetLength] == Identifier) is false)//endIdentifier 체크
                {
                    //이 경우는 startIdentifier 부터 잘못된것.
                    nBytesProcessed += 1;
                    continue;
                }
                //Console.WriteLine("start3");

                //3단계 이동
                //convertible.Decode(span, ref offset);
                nBytesProcessed += pactetLength + 1; //패킷의 마지막 바이트는 endIdentifier

                if(temp != null)
                {
                    take = temp;
                    temp= null;

                }
                take(typeNumber, span, offset);
            }

            return nBytesProcessed;
        }
    }
}

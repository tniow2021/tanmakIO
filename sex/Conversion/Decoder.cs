using sex.DataStructure;
namespace sex.Conversion
{
    public class Decoder
    {
        DynamicBuff<byte> buff;
        Action<Convertible> take;
        const byte Identifier = 0b11111111;
        ConvertibleGroup convertibleGroup;
        public Decoder(DynamicBuff<byte> buff, Action<Convertible> take,ConvertibleGroup convertibleGroup)
        {
            this.buff = buff;
            this.take = take;
            this.convertibleGroup = convertibleGroup;
        }
        public unsafe void Boxing(Convertible c)
        {
            int n = 2 + 2 + 2 + c.GetLength();
            if (buff.Write(n, out Span<byte> span))
            {
                short typeNumber = c.GetTypeNumber();
                short size = c.GetLength();
                byte* bp = (byte*)&typeNumber;

                span[0] = 0b11111111;
                span[1] = 0b11111111;

                span[2] = *bp;
                span[3] = *(bp + 1);

                bp = (byte*)&size;
                span[4] = *bp;
                span[5] = *(bp + 1);

                int offset = 6;
                c.Encode(span, ref offset);
            }
        }
        enum ProcessState
        {
            IsIdentifier,
            IsTypeNumber,
            IsLengthByte,
            IsData,
            IsUnnown
        }
        ProcessState mode = ProcessState.IsUnnown;
        public unsafe int Unboxing(Span<byte> span)
        {
            if(span.Length<6)
                return 0;

            
            short typeNumber = 0;
            short length = 0;
            int nBytesConverted = 0;

            int i = 0;
            while(true)
            {
                if(span[i] == Identifier)
                {
                    if(span.Length<=i+2+2+1)
                    {
                        return 0;
                    }
                    i += 1;
                    if((span[i] &0b10000000 )>0)//사이즈버퍼의 부호비트가 1이란건 오류라는것
                    {
                        continue;
                    }

                    short* t = &typeNumber;
                    *(t + 0) = span[i];
                    *(t + 1) = span[i + 1];
                    i += 2;

                    t = &length;
                    *(t + 0) = span[i];
                    *(t + 1) = span[i + 1];
                    i += 2;

                    var convertible = convertibleGroup.GetBlock(typeNumber);
                    int minLenth = convertible.GetLength();

                    if(length < minLenth)//패킷에 적힌 사이즈가 디코딩가능 최소크기보다 작을 때.
                    {
                        convertibleGroup.ReturnBlock(convertible);
                        continue;
                    }
                    else if(span.Length - i < minLenth+1)//남은 바이트 수가 사이즈와 끝식별자를 포함한 길이보다 작을 때,
                    {
                        convertibleGroup.ReturnBlock(convertible);
                        return nBytesConverted;
                    }
                    else if (span[i+length+1]!=Identifier)//끝식별자를 찾을 수 없을 때.
                    {
                        convertibleGroup.ReturnBlock(convertible);
                    }
                    else
                    {
                        convertible.Decode(span, ref i);
                        nBytesConverted += i;
                        take(convertible);
                    }
                }
                else
                {
                    i++;
                }
            }
            

            return i;
        }
    }
}

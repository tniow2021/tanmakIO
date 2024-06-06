using sex.DataStructure;
using System.Net;

namespace sex.Conversion
{
    public class Decoder
    {
        DynamicBuff<byte> buff;
        Action<Convertible> take;
        const byte Identifier = 0b11111111;
        public Decoder(DynamicBuff<byte> buff, Action<Convertible> take)
        {
            this.buff = buff;
            this.take = take;
        }
        public void Set(DynamicBuff<byte> buff,) { }
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
        static Func<Convertible>[] takeStruct = new Func<Convertible>[100];
        public unsafe int Unboxing(Span<byte> span)
        {
            if(span.Length<6)
                return 0;

            int i = 0;
            short typeNumber = 0;
            short length = 0;
            int nBytesConverted = 0;

            do
            {
                //switch (mode)
                //{
                //    case ProcessState.IsUnnown:
                //        i++;
                //        mode = ProcessState.IsIdentifier;
                //        break;
                //    case ProcessState.IsIdentifier:
                //        if (span[i] == Identifier && span[i + 1] == Identifier)
                //        {
                //            mode = ProcessState.IsTypeNumber;
                //            i += 2;
                //        }
                //        else
                //        {
                //            mode= ProcessState.IsUnnown;
                //            break;
                //        }
                //    case ProcessState.IsTypeNumber:
                //        

                //        i += 2;
                //        mode = ProcessState.IsLengthByte;
                //        break;
                //    case ProcessState.IsLengthByte:
                //        if (span.Length - i >= length)
                //        {
                //            //이후 수정
                //            Convertible c = takeStruct[typeNumber]();
                //            c.Decode(span, ref i);
                //        }
                //        break;
                //    case ProcessState.IsData:
                //        break;

                //}
                if(span[i] == Identifier)
                {
                    if(span.Length<=i+2+2)
                    {
                        //종료
                    }
                    i += 1;

                    short* t = &typeNumber;
                    *(t + 0) = span[i];
                    *(t + 1) = span[i + 1];
                    i += 2;

                    t = &length;
                    *(t + 0) = span[i];
                    *(t + 1) = span[i + 1];
                    i += 2;

                    int minLenth = 43344;
                    if(length< minLenth)
                        //예외 종료
                }
                else
                {
                    i++;
                }
            }
            while ();
            

            return i;
        }
    }
}

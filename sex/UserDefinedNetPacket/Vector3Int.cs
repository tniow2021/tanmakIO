using sex.Conversion;

namespace sex.UserDefinedNetPacket
{
    public class Vecter3Int : NetConvertible
    {
        public const short length = sizeof(Int32) * 3;
        public Int32 x, y, z;
        static short typeNumber;
        public Vecter3Int(Int32 x, Int32 y, Int32 z)
        {
            this.x = x; this.y = y; this.z = z;
        }
        public Vecter3Int()
        {
            this.x = 0; this.y = 0; this.z = 0;
        }
        public static void SetTypeNumber(short _typenumber)
        {
            typeNumber= _typenumber;
        }
        public short GetLength()
        {
            return length;
        }
        public short GetTypeNumber()
        {
            return typeNumber;
        }
        public unsafe void Encode(Span<byte> span, ref int offset)
        {
            fixed(byte*ptr=span)
            {
                BaseType.ptrEncode<Int32>(x, ptr, ref offset);
                BaseType.ptrEncode<Int32>(y, ptr, ref offset);
                BaseType.ptrEncode<Int32>(z, ptr, ref offset);
            }
        }
        public unsafe void Decode(Span<byte> span, ref int offset)
        {
            fixed (byte* ptr = span)
            {
                BaseType.ptrDecode<Int32>(out x, ptr, ref offset);
                BaseType.ptrDecode<Int32>(out y, ptr, ref offset);
                BaseType.ptrDecode<Int32>(out z, ptr, ref offset);
            }
        }
    }
}

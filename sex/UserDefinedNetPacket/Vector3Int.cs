using sex.Conversion;

namespace sex.UserDefinedNetPacket
{
    public class Vecter3Int : INetConvertible
    {
        public const short length = sizeof(Int32) * 3;
        public Int32 x, y, z;
        public static short typeNumber;
        public Vecter3Int(Int32 x, Int32 y, Int32 z)
        {
            this.x = x; this.y = y; this.z = z;
        }
        public Vecter3Int()
        {
            this.x = 0; this.y = 0; this.z = 0;
        }
        public short GetLength()
        {
            return length;
        }
        public short GetTypeNumber()
        {
            return typeNumber;
        }
        public void Encode(Span<byte> span, ref int offset)
        {
            BaseType.Encode(x, span, ref offset);
            BaseType.Encode(y, span, ref offset);
            BaseType.Encode(z, span, ref offset);
        }
        public void Decode(Span<byte> span, ref int offset)
        {
            BaseType.Decode(out x, span, ref offset);
            BaseType.Decode(out y, span, ref offset);
            BaseType.Decode(out z, span, ref offset);
        }
    }
    public class TestClass : INetConvertible
    {
        public const short length = 3;
        public static short typeNumber;
        public short GetLength()
        {
            return length;
        }
        public short GetTypeNumber()
        {
            return typeNumber;
        }
        public void Encode(Span<byte> span, ref int offset)
        {
            span[offset] = 3;
            span[offset+1] = 4;
            span[offset + 2] = 5;
            offset += 3;
        }
        public void Decode(Span<byte> span, ref int offset)
        {
            Console.WriteLine("값: " + span[offset] + " " + span[offset + 1] + " " + span[offset + 2] + " ");
            offset += 3;
        }
    }
}

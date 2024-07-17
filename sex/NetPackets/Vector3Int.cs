using sex.Conversion;

namespace sex.NetPackets
{
    public struct Vector3Int : INetPacket
    {
        public const short length = sizeof(Int32) * 3;
        public Int32 x, y, z;
        public const short typeNumber = (short)EnumNetPacket.Vector3Int;
        public unsafe Vector3Int(Span<byte> span, ref int offset)
        {
            fixed (byte* ptr = span)
            {
                BaseType.ptrDecode<Int32>(out x, ptr, ref offset);
                BaseType.ptrDecode<Int32>(out y, ptr, ref offset);
                BaseType.ptrDecode<Int32>(out z, ptr, ref offset);
            }
        }
        public unsafe void Encode(Span<byte> span, ref int offset)
        {
            fixed (byte* ptr = span)
            {
                BaseType.ptrEncode<Int32>(x, ptr, ref offset);
                BaseType.ptrEncode<Int32>(y, ptr, ref offset);
                BaseType.ptrEncode<Int32>(z, ptr, ref offset);
            }
        }
        public short GetLength(){ return length; }
        public static short GetMinimumLength(){ return length; }
        public short GetTypeNumber(){ return typeNumber; }

    }
}

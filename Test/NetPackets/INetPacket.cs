namespace sex.NetPackets
{
    public interface INetPacket
    {
        public abstract short GetLength();
        public abstract short GetTypeNumber();
        public abstract void Encode(Span<byte> span, ref int offset);
        public abstract static short GetMinimumLength();
    }
}

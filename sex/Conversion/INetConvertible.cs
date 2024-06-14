using System.Runtime.CompilerServices;

namespace sex.Conversion
{
    public interface INetConvertible
    {
        public abstract short GetLength();
        public abstract short GetTypeNumber();
        public abstract void Encode(Span<byte> span, ref int offset);
        public abstract void Decode(Span<byte> span, ref int offset);
    }
}

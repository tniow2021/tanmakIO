using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sex.Conversion
{
    public  class Decoder
    {
        public void Input(byte[] data, int start, int length)
        {
            Span<byte> sb=new Span<byte>(data, start, length);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sex.DataStructure
{
    public class DynamicBuff<T>
    {
        T[] buff =Array.Empty<T>();
        int L;//Length of the buff
        int w;//쓸 위치
        int r;//읽을 위치
        public DynamicBuff(T[] buff)
        {
            this.buff = buff;
            L = buff.Length;
            w = 0;
            r = 0;
        }
        public DynamicBuff()
        {
            w = 0;
            r = 0;
        }
        public void SetBuff(T[]buff,int writeStartIndex=0,int ReadStartIndex=0)
        {
            this.buff=buff;
            L = buff.Length;
            w = writeStartIndex;
            r = ReadStartIndex;
        }
        public T[] GetBuff()
        {
            return buff;
        }
        public int GetWriteOffset()
        {
            return w;
        }
        public void IncreaseWriteOffset(int plus)
        {
            w+= plus;
        }
        public bool Write(int n, out Memory<T> memory)
        {
            if (n <= L - w)//연속적으로 쓸 수 있는 양과 비교
            {
                memory=new Memory<T>(buff, w, n);
                w += n;
                return true;
            }
            else
            {
                //정리
                Arrange();
                //다시검사
                if (n <= L - w)
                {
                    memory = new Memory<T>(buff, w, n);
                    w += n;
                    return true;
                }
                memory = Memory<T>.Empty;
                return false;
            }
        }
        public bool Write(int n, out Span<T> span)
        {
            if (n <= L - w)//연속적으로 쓸 수 있는 양과 비교
            {
                span = new Span<T>(buff, w, n);
                w += n;
                return true;
            }
            else
            {
                //정리
                Arrange();
                //다시검사
                if (n <= L - w)
                {
                    span = new Span<T>(buff, w, n);
                    w += n;
                    return true;
                }
                span = Span<T>.Empty;
                return false;
            }
        }
        public bool Read(int n, out Span<T> span)
        {
            if (n <= w - r)//읽을 수 있는 양과 비교
            {
                span = new Span<T>(buff, r, n);
                r += n;
                if (w == r)//만약 이후에 더이상 읽을 데이터가 없다
                {
                    w = 0;//그럼 다시 첫위치로
                    r = 0;
                }
                return true;
            }
            span = Span<T>.Empty;
            return false;
        }
        public bool Read(int n, out Memory<T> memory)
        {
            if (n <= w - r)//읽을 수 있는 양과 비교
            {
                memory = new Memory<T>(buff, r, n);
                r += n;
                if (w == r)//만약 이후에 더이상 읽을 데이터가 없다
                {
                    w = 0;//그럼 다시 첫위치로
                    r = 0;
                }
                return true;
            }
            memory = Memory<T>.Empty;
            return false;
        }
        public bool ReadAll(out Span<T> span)
        {
            if(w-r>0)
            {
                span = new Span<T>(buff, r, w - r);
                return true;
            }
            span=Span<T>.Empty;
            return false;
        }
        public bool ReadAll(out Memory<T> memory)
        {
            if (w - r > 0)
            {
                memory = new Memory<T>(buff, r, w - r);
                return true;
            }
            memory = Memory<T>.Empty;
            return false;
        }

        public void Arrange()
        {
            int tempR = r;
            r = 0;
            w = w - r;
            for (int i = 0; i < w - r; i++)
            {
                buff[i] = buff[tempR + i];
            }
        }
        public int GetNumContiguousSpaces()
        {
            return L - w;
        }
        public int GetNumNonContiguousSpaces()
        {
            return L - (w - r);
        }
        public int GetNumCanRead()
        {
            return w - r;
        }
    }
}

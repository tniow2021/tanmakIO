﻿namespace sex.DataStructure
{
    public class DynamicBuff<T>
    {
        static Action<DynamicBuff<T>> empty = (DynamicBuff<T>a) => { };
        T[] buff =Array.Empty<T>();
        int L;//Length of the buff
        int w;//쓸 위치
        int r;//읽을 위치
        public Action<DynamicBuff<T>> fullEvent = empty;
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
        public void SetBuff(T[]buff)
        {
            this.buff = buff;
        }
        public void SetBuff(T[]buff,int writeStartIndex,int ReadStartIndex)
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
            if(w+plus>L-1)
            {
                Arrange();
                if(w>=L-1)
                {
                    fullEvent(this);
                    //throw new Exception("dynamic buff is max");
                }
                w+= plus;
                return;
            }
            w += plus;
        }
        public void IncreaseReadOffset(int plus)
        {
            if(r+plus<=w)
            {
                r+= plus;
                return;
            }
            throw new Exception("dynamic buff. IncreaseReadOffset() error");
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
                fullEvent(this);
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
                fullEvent(this);
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
            if (w-r>0)
            {
                span = new Span<T>(buff, r, w - r);
                r = 0;
                w = 0;
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
                r = 0;
                w = 0;
                return true;
            }
            memory = Memory<T>.Empty;
            return false;
        }
        public bool NonCountingRead(out Span<T> span)
        {
            if (w - r > 0)
            {
                span = new Span<T>(buff, r, w - r);
                return true;
            }
            span = Span<T>.Empty;
            return false;
        }
        public void Arrange()
        {
            Console.WriteLine($"정리전-w:{w}-r{r}------------------------------------------------------------");
            int tempR = r;
            w = w - r;
            r = 0;
            for (int i = 0; i < w - tempR; i++)
            {
                buff[i] = buff[tempR + i];
            }
            Console.WriteLine($"정리후-w:{w}-r{r}-------------------------------------------------------------");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class NonContiguousBuffer<T>
    {
        T[] arr;
        int s1, e1;
        int s2, e2;
        public int length { private set; get; }
        public NonContiguousBuffer(T[] arr, int start1, int end1, int start2, int end2)
        {
            this.arr = arr;
            this.s1 = start1;
            this.e1 = end1;

            this.s2 = start2;
            this.e2 = end2;

            if ( //두 영역이 겹친다면
                !((s1 <= e1 && e1 < s2 && s2 <= e2)||(s2 <= e2 && e2 < s1 && s1 <= e1))
                )
            {
                throw new Exception("NonContiguousBuffer 지정한 두영역이 겹침");
            }
            length = (e1 - s1) + 1 + (e2 - s2)+1;
        }
        public T this[int i]
        {
            get
            {
                i += s1;
                if (i > e1)
                {
                    i -= e1+1;
                    i += s2;
                    if (i > e2)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else
                    {
                        return arr[i];
                    }
                }
                else
                {
                    return arr[i];
                }
            }
            set
            {
                i += s1;
                if (i > e1)
                {
                    i -= e1+1;
                    i += s2;
                    if (i > e2)
                    {
                        throw new IndexOutOfRangeException();
                    }
                    else
                    {
                        arr[i] = value;
                    }
                }
                else
                {
                    arr[i] = value;
                }
            }
        }
    }
}

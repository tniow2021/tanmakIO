using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class QPool<T>
    {
        T[] arr;
        public UInt32 w;//미래에 쓸 위치
        public UInt32 nSpace;//비어있는 공간수
        UInt32 L;
        UInt32 LastIndex;
        Func<T> constructor;
        public QPool(Func<T> constructor, int n)
        {
            this.constructor = constructor;
            arr = new T[n];
            if (arr == null)
                throw new Exception("pool error 1");
            if (n > Int32.MaxValue)
                throw new Exception("pool error 1-2");

            for (int i = 0; i < n; i++)
            {
                var t = constructor();
                if (t != null)
                {
                    w++;
                    arr[i] = t;
                }
                else
                    throw new Exception("pool error 2-2");
            }
            w = 0;
            L = (UInt32)arr.Length;
            LastIndex = L - 1;
            nSpace = 0;
        }
        public T GetBlock()
        {
            if (L - nSpace > 0)//데이터량>0
            {
                UInt32 r = w + nSpace;
                if (r > LastIndex)
                    r -= L;
                nSpace += 1;
                return arr[r];
            }
            else
                return constructor();

        }
        public void RepayBlock(T t)
        {
            if (t is null)
            {
                return;
            }

            if (nSpace > 0)//공간량>0
            {
                arr[w] = t;
                w += 1;
                if (w > LastIndex)
                    w -= L;
                nSpace -= 1;
            }
        }

        public void Display()
        {
            string s = ".";
            for (int i = 0; i < L; i++)
            {
                if (i == w && i == (w + nSpace) % L)
                {
                    s += "wr.";
                }
                else if (i == w)
                {
                    s += "w .";
                }
                else if (i == (w + nSpace) % L)
                {
                    s += "r .";
                }
                else
                {
                    s += "  .";
                }
            }
            s += "   " + nSpace + "\n";
            Console.WriteLine(s);
        }
    }


    public class StackPool<T>
    {
        T[] arr;
        public UInt32 count;
        UInt32 lastIndex;
        Func<T> constructor;
        public StackPool(Func<T> constructor, int n)
        {
            this.constructor = constructor;
            arr = new T[n];
            if (arr == null)
                throw new Exception("pool error 1");

            count = 0;
            for (int i = 0; i < n; i++)
            {
                var t = constructor();
                if (t != null)
                {
                    count++;
                    arr[i] = t;
                }
                else
                    throw new Exception("pool error 2-2");
            }
            lastIndex =(UInt32) arr.Length - 1;
        }
        public T GetBlock()
        {
            if (count > 0)
            {
                T t = arr[count - 1];
                count--;
                return t;
            }
            else
            {
                var t = constructor();
                return t;
            }
        }
        public void RepayBlock(T t)
        {
            if (t != null)
            {
                if (count > lastIndex )
                {
                    return;
                }
                arr[count] = t;
                count++;
            }
        }

        public void Display()
        {
            string s = ".";
            for (int i = 0; i < arr.Length; i++)
            {
                AA a = arr[i]as AA;
                s += a.a + " .";
            }
            s +="\n";
            Console.WriteLine(s);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class qPool<T>
    {
        T[] arr;
        public UInt32 w;//미래에 쓸 위치
        public UInt32 nSpace;//데이터양
        UInt32 L;
        Func<T> constructor;
        public qPool(Func<T> constructor, int n)
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
            nSpace = 0;
        }
        public T GetBlock()
        {
            if (L - nSpace > 0)//데이터량>0
            {
                UInt32 r = (w + nSpace) % L;
                nSpace += 1;
                Console.WriteLine($"위치 {r}에서 받아오기");
                return arr[r];
            }
            else
                Console.WriteLine("섹스");
                return constructor();

        }
        public void RepayBlock(T t)
        {
            if (t is null)
            {
                Console.WriteLine("섹스2");
                return;
            }

            if (nSpace > 0)//공간량>0
            {
                Console.WriteLine($"위치 {w}에 쓰고");
                arr[w] = t;
                Console.WriteLine($"w는 {(w + 1) % L}가 된다,");
                w = (w + 1) % L;
                nSpace -= 1;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public struct RingBuffInfo
    {
        public int nSpaceRemaining;
        public int nDataRemaining;
        public RingBuffInfo(
            int nSpaceRemaining,
            int nDataRemaining
            )
        {
            this.nSpaceRemaining = nSpaceRemaining;
            this.nDataRemaining = nDataRemaining;
        }
    }

    //n은 쓸 수 있는 요소의 수
    public delegate int TakeAndProcess<T>(Span<T> sp,int n);
    public class RingBuff<T>
    {
        public T[] buff { private set; get; }
        int L { get; }//Length of the buff
        int w;//current Write Index
        int r;//current Read Index
        //int l;//Index of the last data
        public RingBuff(T[] buff)
        {
            this.buff = buff;
            L=buff.Length;
            w = 0; //쓸 위치(미래형)
            r = -1;//쓴 위치(과거형)
            l = -1;
        }
        public RingBuffInfo GetInfo()
        {
            return new RingBuffInfo();
        }
        public int Read(TakeAndProcess<T> tp)
        {
            int n = 읽을수있는수계산();
            int nProcessed = tp(new Span<T>(buff,r+1,n),n);
            r=(r+1+nProcessed)%L;
            return 읽을수있는수계산();
        }
        public bool Read(TakeAndProcess<T> tp,int nSize,out int nDataRemaining)
        {
            int n = 읽을수있는수계산();
            if(nSize<n)//처리
            {
                int nProcessed = tp(new Span<T>(buff, r + 1, n),n);
                w = (w + nProcessed) % L;
                nDataRemaining= 읽을수있는수계산();
                return true;
            }
            else
            {
                nDataRemaining = n;
                return false;
            }
        }
        public int Write(TakeAndProcess<T> tp)
        {
            int n = 쓸수있는수계산();
            int nProcessed = tp(new Span<T>(buff, r + 1, n),n);
            w = (w + nProcessed) % L;
            return 쓸수있는수계산();
        }
        public bool Write(TakeAndProcess<T> tp, int nSize, out int nDataRemaining)
        {
            int n = 쓸수있는수계산();
            if(nSize<=n)//처리
            {
                int nProcessed = tp(new Span<T>(buff, r + 1, n),n);
                w=(w+nProcessed)%L;
                nDataRemaining= 쓸수있는수계산();
                return true;
            }
            else//처리불가
            {
                nDataRemaining= n;
                return false;
            }
            
        }

        int 읽을수있는수계산()
        {
            if(r<w)//정방향
            {
                return w - r - 1;
            }
            else if(w<r)
            {
                return L - r - 1;
            }
            else//r=w
            {
                return L;
            }

        }
        int 쓸수있는수계산()
        {
            if (r < w)//정방향
            {
                return L - w;
            }
            else if (w < r)
            {
                return r - w + 1;
            }
            else//r=w
            {
                return 0;
            }
        }
    }
}



  //허락
        //getㅖPIece
        //통보
        //정보구하기












//int theLength,
//            int currentWriteIndex,
//            int currentReadIndex,
//            int lastDataIndex
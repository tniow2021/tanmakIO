using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public struct PieceOfRingBuff
    {
        public PieceOfRingBuff(int buffLength,int startIndex,int count)
        {
            this.buffLength = buffLength;
            this.startIndex = startIndex;
            this.count = count;
        }
        public int buffLength;
        public int startIndex;
        public int count;
    }
    internal class eff
    {
        byte[] buff = new byte[10000];
        // for ring buff system
        int L;// Length;
        int w;// writeIndex;
        int r;// readIndex;
        bool wFirst;//When w and r are the same
        public eff()
        {
            L = buff.Length;
            w= 0;
            r=0;
            wFirst = true;
        }
        //넣을 수 있는 갯수  (넣을려하는 갯수)
        // 빼낼 수 있는 갯수   ( 빼낼려하는 갯수)
        public PieceOfRingBuff Write(int count)
        {
            PieceOfRingBuff pb=new PieceOfRingBuff(L,w,count:0);
            if (r<w)//w값 변화의 결과는 버퍼를 넘을 수도 있다. (L나머지 연산을 해줌)
            {
                int cEmpty = r + (L - w);//공간량
                if (count <= cEmpty)
                {
                    pb.count=count;
                    w=(w+ count) %L;
                }
                else//공간량보다 초과
                {
                    pb.count= cEmpty;
                    w=(w+ cEmpty) %L;
                }
                if(w<r)//중요.  뒤집어졌으면 이 이후로 w과 r이 겹치면 r이 우선,
                        wFirst = false;
            }
            else if(w<r)//버퍼를 넘을 일이 없다.
            {
                int cEmpty = r-w;//공간량
                if(count <= cEmpty)
                {
                    pb.count = count;
                    w = w + count;
                }
                else//공간량보다 초과
                {
                    pb.count= cEmpty;
                    w = w + cEmpty;
                }
            }
            else//r=w
            {
                if(wFirst)//r<w랑 똑같다.
                {
                    int cEmpty = r + (L - w);//공간량
                    if (count <= cEmpty)
                    {
                        pb.count = count;
                        w = (w + count) % L;
                    }
                    else//공간량보다 초과
                    {
                        pb.count = cEmpty;
                        w = (w + cEmpty) % L;
                    }
                    if (w < r)//중요.  뒤집어졌으면 이 이후로 w과 r이 겹치면 r이 우선,
                        wFirst = false;
                }
                else//w<r랑 똑같다.
                {
                    int cEmpty = r - w;//공간량
                    if (count <= cEmpty)
                    {
                        pb.count = count;
                        w = w + count;
                    }
                    else//공간량보다 초과
                    {
                        pb.count = cEmpty;
                        w = w + cEmpty;
                    }
                }
            }
            return pb;
        }
        public void Read(int count)
        {
            PieceOfRingBuff pb = new PieceOfRingBuff(L, r, 0);
            if(r<w)//r값의 변화는 버퍼를 넘을 일이 없다.
            {
                int nData = w - r;//데이터량
                if(count<= nData)
                {
                    pb.count = count;
                    r = r + count;
                }
                else//데이터량보다 초과
                {
                    pb.count = nData;
                    r = r + nData;
                }
            }
            else if(w<r)//r값의 변화가 버퍼를 넘을 수도 있다.(L나머지 연산을 해줌)
            {
                int nData = w +(L- r);//데이터량
                if(count<= nData)
                {
                    pb.count= count;
                    r = (r + count) % L;
                }
                else//데이터량보다 초과
                {
                    pb.count = nData;
                    r = (r + nData) % L;
                }
                if (r>w)//중요.  뒤집어졌으면 이 이후로 w과 r이 겹치면 w이 우선,
                    wFirst = true;
            }
            else//r=w
            {
                if(wFirst)//r<w와 같다
                {
                    if (r < w)//r값의 변화는 버퍼를 넘을 일이 없다.
                    {
                        int nData = w - r;//데이터량
                        if (count <= nData)
                        {
                            pb.count = count;
                            r = r + count;
                        }
                        else//데이터량보다 초과
                        {
                            pb.count = nData;
                            r = r + nData;
                        }
                    }
                }
                else//w<r과 같다,//r값의 변화가 버퍼를 넘을 수도 있다.(L나머지 연산을 해줌)
                {
                    int nData = w + (L - r);//데이터량
                    if (count <= nData)
                    {
                        pb.count = count;
                        r = (r + count) % L;
                    }
                    else//데이터량보다 초과
                    {
                        pb.count = nData;
                        r = (r + nData) % L;
                    }
                    if (r > w)//중요.  뒤집어졌으면 이 이후로 w과 r이 겹치면 w이 우선,
                        wFirst = true;
                }
            }
        }
    }

}


































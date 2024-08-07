﻿namespace sex.Conversion
{
    public static class BaseType
    {
        //generic
        //static unsafe void Encode<T>(T value, MemoryStream ms) where T : unmanaged
        //{
        //    byte* valuePtr = (byte*)&value;
        //    for (int i = 0; i < sizeof(T); i++)
        //    {
        //        ms.WriteByte(*(valuePtr + i));
        //    }
        //}
        //static unsafe void Decode<T>(out T t, MemoryStream ms) where T : unmanaged
        //{
        //    T v;
        //    byte* ptr = (byte*)&v;
        //    for (int i = 0; i < sizeof(T); i++)
        //    {
        //        *(ptr + i) = (byte)ms.ReadByte();
        //    }
        //    t = v;
        //}


        public static unsafe void ptrEncode<T>(T v, byte* ptr, ref int offset)where T:unmanaged
        {
            *(T*)(ptr + offset) = v;
            offset += sizeof(T);
        }
        public static unsafe void ptrDecode<T>(out T v, byte* ptr, ref int offset) where T : unmanaged
        {
            v = *((T*)(ptr + offset));
            offset += sizeof(T);
        }
        //개별 타입
        //int
        public static unsafe void Encode(Int32 value, Span<byte> span, ref int offset)
        {
            byte* valuePtr = (byte*)&value;
            span[offset + 0] = *(valuePtr + 0);
            span[offset + 1] = *(valuePtr + 1);
            span[offset + 2] = *(valuePtr + 2);
            span[offset + 3] = *(valuePtr + 3);
            offset += 4;
        }
        public static unsafe void Decode(out int value, Span<byte> span, ref int offset)
        {
            int v;
            byte* ptr = (byte*)&v;
            *(ptr + 0) = span[offset + 0];
            *(ptr + 1) = span[offset + 1];
            *(ptr + 2) = span[offset + 2];
            *(ptr + 3) = span[offset + 3];
            value = v;
            offset += 4;
        }
        //float
        public static unsafe void Encode(float value, Span<byte> span, ref int offset)
        {
            byte* valuePtr = (byte*)&value;
            span[offset + 0] = *(valuePtr + 0);
            span[offset + 1] = *(valuePtr + 1);
            span[offset + 2] = *(valuePtr + 2);
            span[offset + 3] = *(valuePtr + 3);
            offset += 4;
        }
        public static unsafe void Decode(out float value, Span<byte> span, ref int offset)
        {
            float v;
            byte* ptr = (byte*)&v;
            *(ptr + 0) = span[offset + 0];
            *(ptr + 1) = span[offset + 1];
            *(ptr + 2) = span[offset + 2];
            *(ptr + 3) = span[offset + 3];
            value = v;
            offset += 4;
        }

        //double
        //long
        //public static void Encode(bool value, MemoryStream ms)
        //{
        //    ms.WriteByte((byte)(value ? 1 : 0));//1 is true, 0 is false
        //}
        //public static void Decode(out bool value, MemoryStream ms)
        //{
        //    value = (ms.ReadByte() == 0) ? false : true;
        //}
        ////string
        //public static void Encode(string s, MemoryStream ms)
        //{
        //    Span<byte> bs = Encoding.Unicode.GetBytes(s);

        //    Encode(bs.Length, ms);//문자열 사이즈 먼저기록
        //    ms.Write(bs);
        //}
        //public static unsafe void Decode(out string s, MemoryStream ms)
        //{
        //    int bytesLength = 0;
        //    Decode(out bytesLength, ms);
        //    Span<byte> bs = stackalloc byte[bytesLength];
        //    ms.Read(bs);
        //    s = Encoding.Unicode.GetString(bs);
        //}
        ////generic array
        //public static void Encode<T>(Span<T> sp, MemoryStream ms) where T : unmanaged
        //{
        //    BaseType.Encode<Int32>(sp.Length, ms);
        //    for (int i = 0; i < sp.Length; i++)
        //    {
        //        //인라인으로 하면 더빠를 수 있다.
        //        BaseType.Encode<T>(sp[i], ms);
        //    }
        //}
        //public static void Encode<T>(List<T> ls, MemoryStream ms) where T : unmanaged
        //{
        //    BaseType.Encode<Int32>(ls.Count, ms);
        //    for (int i = 0; i < ls.Count; i++)
        //    {
        //        //인라인으로 하면 더빠를 수 있다.
        //        BaseType.Encode<T>(ls[i], ms);
        //    }
        //}
        /// <summary>
        /// 디코딩에 필요한 배열요소수를 반환.
        /// 매개변수로 보낸 배열의 크기와 반환값이 같을시 디코딩 성공
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sp"></param>
        /// <param name="ms"></param>
        /// <returns></returns>
        //    public static int Decode<T>(Span<T> sp, MemoryStream ms) where T : unmanaged
        //    {
        //        int lenght;
        //        BaseType.Decode(out lenght, ms);
        //        if (sp.Length != lenght)
        //        {
        //            int temp = lenght;
        //            ms.Position -= sizeof(Int32);
        //            BaseType.Encode(lenght, ms);
        //            return temp;
        //        }
        //        for (int i = 0; i < lenght; i++)
        //        {
        //            BaseType.Decode<T>(out sp[i], ms);
        //        }
        //        return lenght;
        //    }
        //}
        /*
        public static class DynamicType
        {
            public static bool Encode<T>(List<T> sp, MemoryStream ms) where T : Convertible
            {
                if(sp is null)return false;
                BaseType.Encode((Int32)sp.Count, ms);
                for (int i = 0; i < sp.Count; i++)
                {
                    sp[i].Encode(ms);
                }
                return true;
            }
            /// <summary>
            /// 디코딩시 sp가 작을 경우 크기를 자동으로 늘림
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="sp"></param>
            /// <param name="ms"></param>
            /// <returns>디코딩한 배열요소수</returns>
            public static int Decode<T>(List<T> sp, MemoryStream ms) where T : Convertible,new()
            {
                int count;
                BaseType.Decode(out count, ms);
                for(int i=0;i<sp.Count;i++)
                {
                    sp[i].Decode(ms);
                }
                if(count>sp.Count)
                {
                    int addNumber = count - sp.Count;
                    for (int i=0;i< addNumber; i++)
                    {
                        T t=new T();
                        t.Decode(ms);
                        sp.Add(t);
                    }
                }
                return count;
            }
        }
        public delegate IsSuccess Decode(Span<byte> sb, out Convertible cv);
        public static class ConversionTable
        {
            public static List<Decode> decodes=new List<Decode>();
            public static void RegisterDecodeFuntion(Decode funtion,uint number)
            {
                if(decodes.Count<=number)
                {
                    for(int i=0;i<number-decodes.Count;i++)
                    {
                        decodes.Add((Span<byte> sb, out Convertible cv) => { cv = null; return IsSuccess.failure; });
                    }
                }
            }
        }
        */
    }
}
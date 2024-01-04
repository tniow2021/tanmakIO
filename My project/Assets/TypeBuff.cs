using System;
using System.Collections.Generic;
using UnityEngine;

public class TypeBuff
{
    /*
     * 바이너리 데이터 구조 형식:
     * -데이터+구분코드-
     * 
     * 네트워크 구조체 최대 갯수:0~255 총 256개
     * 이유: 구조체구분코드가 1바이트이기때문
     *
     *(추후) 주고받는 바이너리형태를 좀더 유연하고 런타임중 원격제어가능하게 만들 순 없을까. 
     */
    public enum TypeCode
    {
        UserTransform=0
    }
    public static byte TypeCodeToByte(TypeCode tp)
    {
        return (byte)tp;
    }
    public static TypeCode ByteToTypeCode(byte b)
    {
        return (TypeCode)b;
    }
    public interface INetStruct
    {
        public byte[] Encoding();
        public void Decoding(byte[] data);
    }

    public struct UserTransform: INetStruct
    {
        //유저 식별자
        public float x, y;
        public UserTransform(float _x,float _y)
        {
            x = _x;
            y = _y;
        }
        public byte[] Encoding()
        {
            int e;
            byte[] data = new byte[4 * 2];
            System.BitConverter.GetBytes(x).CopyTo(data, 0);
            System.BitConverter.GetBytes(y).CopyTo(data, 4);
            return data;
        }
        public void Decoding(byte[] data)
        {
            x = System.BitConverter.ToSingle(data, 0);
            y = System.BitConverter.ToSingle(data, 4);
        }
    }
    Queue<UserTransform> Queue_UserTransform = new Queue<UserTransform>();

    public void Push(byte[]data)
    {
         /*
         * 구조체의 디코딩은 바이트 배열의 시작부터 해석하고
         * 구분코드는 마지막에 있기 때문에 불필요하게 배열을 자를 필요가 없음.
         */
        byte CodeByte = data[data.Length-1];//마지막 인덱스

        switch (ByteToTypeCode(CodeByte))//핵심코드 
        {
            case TypeCode.UserTransform:
                UserTransform u= new UserTransform();
                u.Decoding(data);
                Queue_UserTransform.Enqueue(u);//큐에 삽입
                break;
        }
    }


    //여러개의 Pull함수들 
    public void pull(out UserTransform ut)
    {
        ut= Queue_UserTransform.Dequeue();
    }
}

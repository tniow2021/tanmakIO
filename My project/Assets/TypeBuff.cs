using System;
using System.Collections.Generic;
using UnityEngine;

public class TypeBuff
{
    /*
     * ���̳ʸ� ������ ���� ����:
     * -������+�����ڵ�-
     * 
     * ��Ʈ��ũ ����ü �ִ� ����:0~255 �� 256��
     * ����: ����ü�����ڵ尡 1����Ʈ�̱⶧��
     *
     *(����) �ְ�޴� ���̳ʸ����¸� ���� �����ϰ� ��Ÿ���� ����������ϰ� ���� �� ������. 
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
        //���� �ĺ���
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
         * ����ü�� ���ڵ��� ����Ʈ �迭�� ���ۺ��� �ؼ��ϰ�
         * �����ڵ�� �������� �ֱ� ������ ���ʿ��ϰ� �迭�� �ڸ� �ʿ䰡 ����.
         */
        byte CodeByte = data[data.Length-1];//������ �ε���

        switch (ByteToTypeCode(CodeByte))//�ٽ��ڵ� 
        {
            case TypeCode.UserTransform:
                UserTransform u= new UserTransform();
                u.Decoding(data);
                Queue_UserTransform.Enqueue(u);//ť�� ����
                break;
        }
    }


    //�������� Pull�Լ��� 
    public void pull(out UserTransform ut)
    {
        ut= Queue_UserTransform.Dequeue();
    }
}

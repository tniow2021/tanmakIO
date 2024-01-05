using System;
using UnityEngine;

public class BinaryHandler
{
    byte Trigger;
    byte[] binaryBuff;
    public BinaryHandler(byte cutTrigger)
    {
        Trigger = cutTrigger;
    }
    enum CutState
    {
        GettingSpliter,
        GettingSizeByte,
        GettingData
    }
    CutState cutState = CutState.GettingSpliter;
    const int sizeByteSize = 4;//int���� ������
    int GettingSizeByte_Count = 0;//���������Ʈ�� �޴� ����Ʈ�� ���¿뵵 

    byte[] SizeByteBuff = new byte[sizeByteSize];//int�� 4����Ʈ ����
    int DataSizeCount = 0;//SizeByteBuff�� int�� ��ȯ�� ����.
    public bool UnPack(byte b, out byte[] binary)
    {
        MonoBehaviour.print(-3);
        //if���� ������� ���������� �ڵ尡 ����.
        if (cutState == CutState.GettingSpliter)
        {
            MonoBehaviour.print(-4);
            if (b == Trigger)
            {
                cutState = CutState.GettingSizeByte;
                GettingSizeByte_Count = 0;
            }
        }
        else if (cutState == CutState.GettingSizeByte)
        {
            MonoBehaviour.print(-5);
            SizeByteBuff[GettingSizeByte_Count] = b;
            GettingSizeByte_Count++;
            if (GettingSizeByte_Count >= sizeByteSize)//sizeByte�� ũ���� 4�� �Ѿ��
            {
                cutState = CutState.GettingData;
                DataSizeCount = System.BitConverter.ToInt32(SizeByteBuff);

                binaryBuff = new byte[DataSizeCount];
            }
        }
        else if (cutState == CutState.GettingData)
        {
            MonoBehaviour.print(-6);
            binaryBuff[binaryBuff.Length - DataSizeCount] = b;
            DataSizeCount--;
            if (DataSizeCount <= 0)
            {
                //�Ϸ��� �ٽ� ��������.
                cutState = CutState.GettingSpliter;
                binary = binaryBuff;
                return true;
            }
        }

        binary = null;
        return false;
    }
    public byte[] Pack(byte[]binary)
    {
        if(binary.Length<=0) return new byte[0];
        //���̳ʸ� ����: ������(1����Ʈ)+������(4����Ʈ)+������
        byte[] b=new byte[binary.Length+1+4];
        byte[] sizeBytes = BitConverter.GetBytes((int)binary.Length);
        b[0] = Trigger;
        for(int i=0;i<4;i++)
        {
            b[1 + i] = sizeBytes[i];
        }
        for(int i=0;i< binary.Length;i++)
        {
            b[1 + 4 + i] = binary[i];
        }
        return b;
    }
}
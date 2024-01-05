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
    const int sizeByteSize = 4;//int형의 사이즈
    int GettingSizeByte_Count = 0;//사이즈바이트를 받는 바이트를 세는용도 

    byte[] SizeByteBuff = new byte[sizeByteSize];//int용 4바이트 버퍼
    int DataSizeCount = 0;//SizeByteBuff를 int로 변환해 저장.
    public bool UnPack(byte b, out byte[] binary)
    {
        MonoBehaviour.print(-3);
        //if문의 순서대로 순차적으로 코드가 진행.
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
            if (GettingSizeByte_Count >= sizeByteSize)//sizeByte의 크기인 4를 넘어가면
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
                //완료후 다시 원점으로.
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
        //바이너리 형식: 구분자(1바이트)+사이즈(4바이트)+데이터
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
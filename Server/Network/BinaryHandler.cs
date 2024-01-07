using System;
public class BinaryHandler
{
    //데이터 형식은 "트리거바이트+사이즈바이트+데이터"입니다.
    //Pack()은 매개로 받은 바이트열을 데이터형식대로 감쌉니다.
    //UnPack()은 매개로 받은 바이트를 기억하고 데이터형식이 다갖춰지면 true를 반환합니다.
    byte Trigger;
    byte[] binaryBuff;
    public BinaryHandler(byte cutTrigger)
    {
        Trigger = cutTrigger;
    }
    enum CutState//UnPack()에서 쓰이는 열거형입니다
    {
        GettingSpliter,//다음에 받을 바이트는 트리거바이트
        GettingSizeByte,//다음에 받을 바이트는 사이즈바이트(총4개:int형)
        GettingData//다음에 받을 바이트는 데이터 바이트
    }
    CutState cutState = CutState.GettingSpliter;
    const int sizeByteSize = 4;//int형의 사이즈
    int GettingSizeByte_Count = 0;//사이즈바이트를 받는 바이트를 세는용도 
    byte[] SizeByteBuff = new byte[sizeByteSize];//int용 4바이트 버퍼
    int DataSizeCount = 0;//SizeByteBuff를 int로 변환해 저장.
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
    public bool UnPack(byte b, out byte[] binary)
    {
        //if문의 순서대로 순차적으로 코드가 진행.
        if (cutState == CutState.GettingSpliter)
        {
            if (b == Trigger)
            {
                cutState = CutState.GettingSizeByte;
                GettingSizeByte_Count = 0;
            }
        }
        else if (cutState == CutState.GettingSizeByte)
        {
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
}
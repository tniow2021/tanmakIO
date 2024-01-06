using System.Collections.Generic;

public class TypeBuff
{
    /*
     * 
     * 게임내 스크립트에서 데이터를 서버에 전송하려면:Push( INetStruct )
     * 게임내 스크립트가 데이터를 가져오려면:Pull()
     * 
     * 네트워크 객체가 바이너리를 줄려면:BinaryPush(byte[])
     * 네트워크 객체가 바이너리를 가져오려면:BinaryPull()
     * ->INetStruct로 디코딩후 종류에 따라RecieveQueue[]에 적재
     * 
     * 
     * 
     * 
     * 바이너리 데이터 구조 형식:
     * -데이터+구분코드-
     * 
     * 네트워크 구조체 최대 갯수:0~255 총 256개
     * 이유: 구조체구분코드가 1바이트이기때문
     *
     *(추후) 주고받는 바이너리형태를 좀더 유연하고 런타임중 원격제어가능하게 만들 순 없을까. 
     */

    //enum형식 TypeCode의 요소 수만큼 Queue를 만든다.
    public Queue<INetStruct>[] recieveQueues
        =new Queue<INetStruct>[System.Enum.GetValues(typeof(TypeCode)).Length];
    public Queue<byte[]> SendQueues
        = new Queue<byte[]>();
    public TypeBuff()
    {
        for (int i=0;i< System.Enum.GetValues(typeof(TypeCode)).Length;i++)
        {
            recieveQueues[i] = new Queue<INetStruct>();
        }
    }
    public void BinaryPush(byte[]data)
    {
        TypeCode typeCode = Unpacking(data);//언패킹
        switch(typeCode)//구조체 분류
        {
            case TypeCode.UserTransform://나중에일반화할것
            {
                    var u = new UserTransform();
                    u.Decoding(data);
                    recieveQueues[(int)typeCode].Enqueue(u);
                break;
            }
            case TypeCode.AccessRequest:
            {
                var u = new AccessRequest();
                u.Decoding(data);
                recieveQueues[(int)typeCode].Enqueue(u);
                    break;
            }
            case TypeCode.AccessRequestAnswer:
            {
                var u = new AccessRequestAnswer();
                u.Decoding(data);
                recieveQueues[(int)typeCode].Enqueue(u);
                break;
            }
            case TypeCode.TimeOutCherk:
            {
                var u = new TimeOutCherk();
                u.Decoding(data);
                recieveQueues[(int)typeCode].Enqueue(u);
                break;
            }
            case TypeCode.DummyData:
            {
                var u = new DummyData();
                u.Decoding(data);
                recieveQueues[(int)typeCode].Enqueue(u);
                break;
            }
        }
    }
    public bool BinaryPull(out byte[]sendData)
    {
        if(SendQueues.Count>0)
        {
            sendData = SendQueues.Dequeue();
            return true;
        }
        else
        {
            sendData = null;
            return false;
        }
    }
    public void Push(INetStruct ns)
    {
        byte[] sendData = packing(ns.Encoding(), ns.GetTypeCode());//패킹
        SendQueues.Enqueue(sendData);
    }
    public bool pull(out INetStruct st, TypeCode typeCode)
    {
        if(recieveQueues[(int)typeCode].Count>0)
        {
            st= recieveQueues[(int)typeCode].Dequeue();
            return true;
        }
        else
        {
            st = null;
            return false;
        }
    }
    public byte[] packing(byte[]data,TypeCode tc)
    {
        byte[]box= new byte[data.Length+1];
        for(int i=0;i<data.Length;i++)
        {
            box[i]= data[i];
        }
        box[box.Length-1]=Types.TypeCodeToByte(tc);
        return box;
    }
    TypeCode Unpacking(byte[]data)
    {
        return Types.ByteToTypeCode(data[data.Length-1]);//마지막 인덱스.
    }
}

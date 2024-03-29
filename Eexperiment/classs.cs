using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
class test
{
    delegate T SSS<T>();
    void ss()
    {
        SSS<r> sss
        


        TypePort tt=new TypePort();

        Type r = tt.GetType();
        
    }
}
struct dd
{
    char[] ff=new char[5];
    public dd()
    {

    }
}

public class PacketForm
{
    //구분자
    //데이터
    //데이터사이즈(4)
    //타입넘버(4)
    //고정동적여부(1)// 동적1, 고정:2

    /*TypePort 단계:
     * 데이터,데이터사이즈(4),타입넘버(4),타입넘버(4),고정동적여부(1)// 동적1, 고정:2
     */

    public byte[] spliter;
    public byte[] data;
    public byte[] dataSize = new byte[4];
    public byte[] typeNumber = new byte[4];
    public byte isDynamic=1;

    public void InputDataSize(int n)
    {
        dataSize=System.BitConverter.GetBytes(n);
    }
    public void InputTypeNumber(int n)
    {
        typeNumber=System.BitConverter.GetBytes(n);
    }
    public void IsDynamic(bool _isDynamic)
    {
        if(_isDynamic)
        {
            isDynamic = 1;
        }
        else
        {
            isDynamic= 2;
        }
    }
}
/*
 * 통신 개체의 종류:
 * 1. 고정길이 데이터
 * 2. 동적길이 데이터
 * 3. 이벤트성 신호
 * 
 * 데이터의 패킹:
 * 데이터+개체종류바이트(1)+타입넘버(4)
 * 
 * 개체종류바이트가
 * 1이면 고정길이 데이터
 * 2이면 동적길이 데이터
 */
public interface IFixedData//구조체에 상속시킬 것
{
    public int GetTypeNumber();
}
public interface IDynamicData//클래스에 상속시킬 것
{
    public int GetTypeNumber();
    public byte[] Encoding();
    public void Decoding(byte[] data);
}
public class TypePort<TypeEnum>where TypeEnum : Enum
{
    struct QueueAndState<T>
    {
        public QueueAndState()
        {
            Q = new Queue<T>();
        }
        public Queue<T> Q;
        public bool IsMax = false;
    }
    QueueAndState<PacketForm> sendQ;
    QueueAndState<PacketForm>[] receiveQ;
    public TypePort()
    {
        //큐 생성
        sendQ = new QueueAndState<PacketForm>();

        int E_Size = System.Enum.GetValues(typeof(TypeEnum)).Length;
        receiveQ = new QueueAndState<PacketForm>[E_Size];
        for (int i = 0; i < E_Size; i++)
        {
            receiveQ[i] = new QueueAndState<PacketForm>();
        }
    }
    //고정길이 데이터를 인코딩하기
    public bool Push(IFixedData t)
    {
        if(sendQ.IsMax)return false;

        PacketForm pf = new PacketForm();
        pf.InputTypeNumber(t.GetTypeNumber());
        pf.data = StructToByte(t);
        pf.InputDataSize(pf.data.Length);
        pf.IsDynamic(false);
        
        sendQ.Q.Enqueue(pf);
        return true;
    }
    //가변길이 데이터를 인코딩하기
    public bool Push(IDynamicData t)
    {
        if (sendQ.IsMax) return false;

        PacketForm pf = new PacketForm();
        pf.InputTypeNumber(t.GetTypeNumber());
        pf.data = t.Encoding();
        pf.InputDataSize(pf.data.Length);
        pf.IsDynamic(true);
        sendQ.Q.Enqueue(pf);
        return true;
    }

    public bool pull<T>(ref T t) where T : struct, IFixedData
    {
        if(receiveQ[t.GetTypeNumber()].Q.Count<=0)return false;

        PacketForm pf = receiveQ[t.GetTypeNumber()].Q.Dequeue();
        t=ByteToStruct<T>(pf.data);
        return true;
    }
    public bool Pull<T>(ref T t) where T : class, IDynamicData, new()
    {
        if (receiveQ[t.GetTypeNumber()].Q.Count <= 0) return false;

        PacketForm pf = receiveQ[t.GetTypeNumber()].Q.Dequeue();
        t.Decoding(pf.data);
        return true;
    }
    static byte[] StructToByte(IFixedData fd)
    {
        int size = Marshal.SizeOf(fd);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(arr.Length);
        Marshal.StructureToPtr(fd, ptr, false);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
    static T ByteToStruct<T>(byte[] buffer) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));

        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T t = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return t;
    }
}
//public class TypePort<E> where E : Enum
//{
//    public interface I
//    {
//        abstract virtual int typeNumber();
//    }
//    abstract class tttttt
//    {
//        abstract static int khbbkbhbh();
//    }


//    struct QueueAndState<T>
//    {
//        public QueueAndState()
//        {
//            Q=new Queue<T>();
//        }
//        public Queue<T> Q;
//        public bool IsMax=false;
//    }
//    QueueAndState<byte[]> sendBuff;
//    QueueAndState<I>[] receiveBuff;

//    //이벤트 배열....


//    //푸쉬
//    //풀
//    //바이너리푸쉬
//    //바이너리풀

//    //이벤트배열에 이벤트등록함수
//    //큐가 제한크기에 도달시 입출력함수들을 제한한다.
//    public TypePort()
//    {
//        //큐 생성
//        sendBuff = new QueueAndState<byte[]>();

//        int E_Size = System.Enum.GetValues(typeof(E)).Length;
//        receiveBuff = new QueueAndState<byte[]>[E_Size];
//        for (int i = 0; i < E_Size; i++)
//        {
//            receiveBuff[i] = new QueueAndState<byte[]>();
//        }
//    }
//    public delegate void Eve<D>(D d);
//    public bool EventRegister<D>(Eve<D> eve) where D : I, new() 
//    {
//    }

//    public bool Pull<T>(ref T t)where T : I, new()
//    {
//        UInt32 typeIndex =T.GetTypeNumber();
//        if(receiveBuff[typeIndex].Q.Count>0)
//        {
//            byte[] b = receiveBuff[typeIndex].Q.Dequeue();
//            ///디코딩
//            t=ByteToStruct<T>(b);
//            return true;
//        }
//        return false;
//    }
//    public bool Push<T>(T t) where T : I, new()
//    {
//        if (sendBuff.IsMax) return false;

//    }
//    public bool BinaryPull(ref byte[] b)
//    {

//    }
//    public void BinaryPush(byte[] b)
//    {

//    }


//    static byte[] Packing(I ii,UInt32 typeCode)
//    {
//        int size = Marshal.SizeOf(ii);
//        byte[] arr = new byte[size+4];
//        IntPtr ptr = Marshal.AllocHGlobal(arr.Length);

//        Marshal.StructureToPtr(ii, ptr, false);
//        Marshal.Copy(ptr, arr, 0, size);
//        Marshal.FreeHGlobal(ptr);

//        byte[] typeCodeByte = System.BitConverter.GetBytes(typeCode);
//        for(int i=0;i<4;i++)
//        {
//            arr[size+i] = typeCodeByte[i];
//        }
//        return arr;
//    }
//    //static void 
//    static T ByteToStruct<T>(byte[] buffer) where T : I
//    {
//        int size = Marshal.SizeOf(typeof(T));

//        if (size > buffer.Length)
//        {
//            throw new Exception();
//        }

//        IntPtr ptr = Marshal.AllocHGlobal(size);
//        Marshal.Copy(buffer, 0, ptr, size);
//        T t = (T)Marshal.PtrToStructure(ptr, typeof(T));
//        Marshal.FreeHGlobal(ptr);
//        return t;
//    }
//    //출처: https://shine10e.tistory.com/117 [열이 Blog : ):티스토리]
//}

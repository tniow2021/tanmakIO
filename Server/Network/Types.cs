using System;
public enum TypeCode
{
    None,
    DummyData,
    TimeOutCherk,
    UserTransform,
    AliveCherk,
    AccessRequest,
    AccessRequestAnswer,
    ExitUserSignal
}
public static class Types
{
    static int numberOfTypeCode = System.Enum.GetValues(typeof(TypeCode)).Length;
    public static byte TypeCodeToByte(TypeCode tp)
    {
        return (byte)tp;
    }
    public static TypeCode ByteToTypeCode(byte b)
    {
        if (b >= numberOfTypeCode) return TypeCode.None;
        return (TypeCode)b;
    }
}
public interface INetStruct
{
    public TypeCode GetTypeCode();
    public byte[] Encoding();
    public void Decoding(byte[] data);
}
//아래는 전부 INetStruct 상속
public struct UserTransform : INetStruct
{
    public int ID;
    public float x, y;
    public UserTransform(float _x, float _y, int _ID)
    {
        ID = _ID;
        x = _x;
        y = _y;
    }
    public TypeCode GetTypeCode() { return TypeCode.UserTransform; }
    public byte[] Encoding()
    {
        byte[] data = new byte[4 * 3];
        System.BitConverter.GetBytes(x).CopyTo(data, 0);
        System.BitConverter.GetBytes(y).CopyTo(data, 4);
        System.BitConverter.GetBytes(ID).CopyTo(data, 8);
        return data;
    }
    public void Decoding(byte[] data)
    {
        x = System.BitConverter.ToSingle(data, 0);
        y = System.BitConverter.ToSingle(data, 4);
        ID = System.BitConverter.ToInt32(data, 8);
    }
}//유저의 좌표값과 ID를 보내고 받을 때 씁니다.
public struct AccessRequest : INetStruct
{
    public int ID;
    public AccessRequest(int _ID)
    {
        ID = _ID;
    }
    public TypeCode GetTypeCode() { return TypeCode.AccessRequest; }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes(ID);
    }
    public void Decoding(byte[] data)
    {
        ID = System.BitConverter.ToInt32(data, 0);
    }
}//서버에게 나를 알립니다
public struct AccessRequestAnswer : INetStruct
{
    public int YourID;
    public AccessRequestAnswer(int _YourID)
    {
        YourID = _YourID;
    }
    public TypeCode GetTypeCode() { return TypeCode.AccessRequestAnswer; }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes(YourID);
    }
    public void Decoding(byte[] data)
    {
        YourID = System.BitConverter.ToInt32(data, 0);
    }

}//서버가 나에게 ID를 줄때 쓰는 응답입니다.
public struct ExitUserSignal : INetStruct
{
    public int exitUserID;
    public ExitUserSignal(int _exitUserID)
    {
        exitUserID = _exitUserID;
    }
    public TypeCode GetTypeCode() { return TypeCode.ExitUserSignal; }
    public byte[] Encoding()
    {
        return System.BitConverter.GetBytes(exitUserID);
    }
    public void Decoding(byte[] data)
    {
        exitUserID = System.BitConverter.ToInt32(data, 0);
    }
}//한 유저가 게임을 나가면 이 구조체가 모두에게 전달됩니다
public struct TimeOutCherk : INetStruct
{
    public TypeCode GetTypeCode() { return TypeCode.TimeOutCherk; }
    public byte[] Encoding()
    {
        return new byte[0];
    }
    public void Decoding(byte[] data) { }
}//연결을 확인하려 할 때 쓰려고 만들었지만 너무 복잡해 아직 미사용입니다
public struct DummyData : INetStruct
{
    public TypeCode GetTypeCode() { return TypeCode.DummyData; }
    public byte[] Encoding()
    {
        return new byte[0];
    }
    public void Decoding(byte[] data) { }
}//비워있는 구조체로 연결을 확인할때 TimeOutCherk대신 썼습니다.
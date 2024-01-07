using System.Collections.Generic;

public class TypeBuff
{
    /*
     * 
     * ���ӳ� ��ũ��Ʈ���� �����͸� ������ �����Ϸ���:Push( INetStruct )
     * ���ӳ� ��ũ��Ʈ�� �����͸� ����������:Pull()
     * 
     * ��Ʈ��ũ ��ü�� ���̳ʸ��� �ٷ���:BinaryPush(byte[])
     * ��Ʈ��ũ ��ü�� ���̳ʸ��� ����������:BinaryPull()
     * ->INetStruct�� ���ڵ��� ������ ����RecieveQueue[]�� ����
     * 
     * 
     * 
     * 
     * ���̳ʸ� ������ ���� ����:
     * -������+�����ڵ�-
     * 
     * ��Ʈ��ũ ����ü �ִ� ����:0~255 �� 256��
     * ����: ����ü�����ڵ尡 1����Ʈ�̱⶧��
     *
     *(����) �ְ�޴� ���̳ʸ����¸� ���� �����ϰ� ��Ÿ���� ����������ϰ� ���� �� ������. 
     */

    //enum���� TypeCode�� ��� ����ŭ Queue�� �����.
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
        TypeCode typeCode = Unpacking(data);//����ŷ
        switch(typeCode)//����ü �з�
        {
            case TypeCode.UserTransform://���߿��Ϲ�ȭ�Ұ�
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
            case TypeCode.ExitUserSignal:
            {
                var u = new ExitUserSignal();
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
    }//����ü�� ���ڵ��� �������� recieveQueues�� ����
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
    }//SendQueues�� ���� ����Ʈ���� ��ȯ
    public void Push(INetStruct ns)
    {
        byte[] sendData = packing(ns.Encoding(), ns.GetTypeCode());//��ŷ
        SendQueues.Enqueue(sendData);
    }//����ü�� ����Ʈ���� ���ڵ��� SendQueues�� ����
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
    }//recieveQueues�� ���� ����ü�� ��ȯ
    //2�� ������ ����:"������+����ü�����ڵ�"
    byte[] packing(byte[]data,TypeCode tc)
    {
        byte[]box= new byte[data.Length+1];
        for(int i=0;i<data.Length;i++)
        {
            box[i]= data[i];
        }
        box[box.Length-1]=Types.TypeCodeToByte(tc);
        return box;
    }//���� ����Ʈ���� ������ ���Ĵ�� ���Ѵ�.
    TypeCode Unpacking(byte[]data)
    {
        return Types.ByteToTypeCode(data[data.Length-1]);//������ �ε���.
    }//���� ����Ʈ���� ��� ����ü�����ڵ带 ��ȯ.
}

using System.Collections.Generic;
using UnityEngine;

public class InGameNet : MonoBehaviour
{
    /*
     * InGameNet�� �ֻ��� ���ӿ�����Ʈ�� �޾��־
     * �� �� ��� ������Ʈ�� ������ �� �ֵ��� �Ѵ�.
     * 
     * player�� AccessRequest�� �ڱ⸦ players�� ����Ѵ�.
     */
    public OtherPlayer originalOther;
    Player player;
    Dictionary<int, OtherPlayer> others = new Dictionary<int, OtherPlayer>();
    void TransformToOthers(UserTransform u)
    {
        if (u.ID == GameManager.Instance.ID) return;
        if(others.ContainsKey(u.ID))//�̹� �ʿ� ������ ������
        {
            others[u.ID].transform.localPosition = Converting.ToVector3(u);
        }
        else//������
        {
            print("���� ���� ���̵�:" + u.ID);
            OtherPlayer clone = Instantiate(originalOther, player.transform.root);
            others.Add(u.ID,clone);
            others[u.ID].transform.localPosition = Converting.ToVector3(u);
        }
    }
    void RemoveUserFormMap(int ID)
    {
        if (others.ContainsKey(ID))//�̹� �ʿ� ������ ������
        {
            print("�����������̵�:" + ID);
            Destroy(others[ID].gameObject);
        }
        else
        {
            print("error:RemoveUserFormMap");
        }
    }

    public void Update()
    {
        TypeBuff typeBuff = GameManager.GetTypeBuff();
        while (typeBuff.pull(out INetStruct st,TypeCode.UserTransform))
        {
            UserTransform ut=(UserTransform)st;
            TransformToOthers(ut);
        }
        while (typeBuff.pull(out INetStruct st, TypeCode.ExitUserSignal))
        {
            ExitUserSignal es = (ExitUserSignal)st;
            RemoveUserFormMap(es.exitUserID);
        }
        SendMyDataToServer(player.transform);
    }
    public void AccessRequest(Player _player)
    {
        player =_player;
        print("��1");
    }

    void SendMyDataToServer(Transform t)
    {
        var u = Converting.ToUserTransForm(t.localPosition);
        u.ID = GameManager.Instance.ID;
        GameManager.GetTypeBuff().Push(u);
    }
}

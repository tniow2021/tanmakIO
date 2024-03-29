using System.Collections.Generic;
using UnityEngine;

public class InGameNet : MonoBehaviour
{
    /*
     * InGameNet�� �ֻ��� ���ӿ�����Ʈ�� �޾��־
     * �� �� ��� ������Ʈ�� ������ �� �ֵ��� �Ѵ�.
     * �� Ŭ������ �ν��Ͻ��� ������� �ʴ´�.
     * player�� AccessRequest�� �ڱ⸦ players�� ����Ѵ�.
     */
    public OtherPlayer originalOther;//�ٸ� ����� ���ӿ� ���� ��� �����Ǵ� ������Ʈ
    Player player;//��
    //���Ӿȿ� �ִ� ����� ��ȸ�ϱ����� �ڷ���. Update���� ���鼭 üũ�ϰ� ���� �־��ݴϴ�.
    Dictionary<int, OtherPlayer> others = new Dictionary<int, OtherPlayer>();
    //�������� ���� ��ǥ������ ���ӿ� �ѷ��� ������Ʈ �մϴ�
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
    //����� ���ӿ��� ������ �� ����� ������Ʈ�� �����մϴ�
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

    private void Start()
    {
        if(GameManager.GetNetwork().IsConnect is false)
        {
            this.enabled = false;
        }
    }
    //Update���� ��� ���� �������� ������ �޽��ϴ�.
    void Update()
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
    //Player(=��)��ü�� �����Ǹ� �ڵ����� �� �Լ��� ȣ���մϴ�
    public void AccessRequest(Player _player)
    {
        player =_player;
        print("��1");
    }
    //�ڽ��� ������ ������ �����մϴ�.
    void SendMyDataToServer(Transform t)
    {
        var u = Converting.ToUserTransForm(t.localPosition);
        u.ID = GameManager.Instance.ID;
        GameManager.GetTypeBuff().Push(u);
    }
}

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
    List<Player> players = new List<Player>();
    private void Start()
    {
        print("�ʱⵥ���� ����");
        GameManager.GetTypeBuff().Push(Converting.ToUserTransForm(new Vector3(3, 4, 5)));
    }

    public void Update()
    {
        if (GameManager.GetTypeBuff().pull(out INetStruct st,TypeCode.UserTransform))
        {
            UserTransform ut=(UserTransform)st;
            print(ut.x + ":" + ut.y);
            Vector3 v3 = Converting.ToVector3(ut);
            print("����������:"+v3);
        }
        foreach (var player in players)
        {
            SendToServer(player.transform);
        }
    }
    public void AccessRequest(Player player)
    {
        players.Add(player);
    }

    void SendToServer(Transform t)
    {
        GameManager.GetTypeBuff().Push(Converting.ToUserTransForm(t.localPosition));
    }
}

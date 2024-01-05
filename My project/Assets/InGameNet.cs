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
    List<TempPlayer> players = new List<TempPlayer>();
    private void Start()
    {
        GameManager.GetTypeBuff().Push(Converting.ToUserTransForm(new Vector3(3, 4, 5)));
    }

    public void Update()
    {
        if(GameManager.GetTypeBuff().pull(out INetStruct st,TypeCode.UserTransform))
        {
            UserTransform ut=(UserTransform)st;
            Vector3 v3 = Converting.ToVector3(ut);
            print("����������:"+v3);
        }
        foreach (var player in players)
        {
            //SendToNetwork(player.transform);
        }
    }
    public void AccessRequest(TempPlayer player)
    {
        players.Add(player);
    }

    void SendToServer(Transform t)
    {
        GameManager.GetTypeBuff().Push(Converting.ToUserTransForm(t));
    }
}

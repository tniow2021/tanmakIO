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
    Player player;
    public List<OtherPlayer>otherPlayers=new List<OtherPlayer>();
    private void Start()
    {

    }

    public void Update()
    {
        TypeBuff typeBuff = GameManager.GetTypeBuff();
        while (typeBuff.pull(out INetStruct st,TypeCode.UserTransform))
        {
            UserTransform ut=(UserTransform)st;
            //print(ut.x + ":" + ut.y);
            Vector3 v3 = Converting.ToVector3(ut);

            foreach(var other in otherPlayers)
            {
                other.transform.localPosition = v3+new Vector3(2,2);
            }
        }
       
        //SendToServer(player.transform);
    }
    public void AccessRequest(Player _player)
    {
        player =_player;
        print("��1");
    }

    void SendToServer(Transform t)
    {
        GameManager.GetTypeBuff().Push(Converting.ToUserTransForm(t.localPosition));
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InGameNet : MonoBehaviour
{
    /*
     * InGameNet은 최상위 게임오브젝트에 달아주어서
     * 씬 속 모든 오브젝트가 접근할 수 있도록 한다.
     * 
     * player는 AccessRequest로 자기를 players에 등록한다.
     */
    public OtherPlayer originalOther;
    Player player;
    Dictionary<int, OtherPlayer> others = new Dictionary<int, OtherPlayer>();
    void TransformToOthers(UserTransform u)
    {
        if (u.ID == GameManager.Instance.ID) return;
        if(others.ContainsKey(u.ID))//이미 맵에 유저가 있으면
        {
            others[u.ID].transform.localPosition = Converting.ToVector3(u);
        }
        else//없으면
        {
            print("들어온 유저 아이디:" + u.ID);
            OtherPlayer clone = Instantiate(originalOther, player.transform.root);
            others.Add(u.ID,clone);
            others[u.ID].transform.localPosition = Converting.ToVector3(u);
        }
    }
    void RemoveUserFormMap(int ID)
    {
        if (others.ContainsKey(ID))//이미 맵에 유저가 있으면
        {
            print("나간유저아이디:" + ID);
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
        print("와1");
    }

    void SendMyDataToServer(Transform t)
    {
        var u = Converting.ToUserTransForm(t.localPosition);
        u.ID = GameManager.Instance.ID;
        GameManager.GetTypeBuff().Push(u);
    }
}

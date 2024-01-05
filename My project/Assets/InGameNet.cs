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
    Player players;
    public List<OtherPlayer>otherPlayers=new List<OtherPlayer>();
    private void Start()
    {
        print("초기데이터 보냄");
        GameManager.GetTypeBuff().Push(Converting.ToUserTransForm(new Vector3(3, 4, 5)));
    }

    public void Update()
    {
        if (GameManager.GetTypeBuff().pull(out INetStruct st,TypeCode.UserTransform))
        {
            UserTransform ut=(UserTransform)st;
            print(ut.x + ":" + ut.y);
            Vector3 v3 = Converting.ToVector3(ut);
            otherPlayer.transform.localPosition = v3+new Vector3(2,2);
            print("받은데이터:"+v3);
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

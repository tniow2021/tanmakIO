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
            print("받은데이터:"+v3);
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

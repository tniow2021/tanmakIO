using System.Collections.Generic;
using UnityEngine;

public class InGameNet : MonoBehaviour
{
    /*
     * InGameNet은 최상위 게임오브젝트에 달아주어서
     * 씬 속 모든 오브젝트가 접근할 수 있도록 한다.
     * 이 클래스의 인스턴스는 변경되지 않는다.
     * player는 AccessRequest로 자기를 players에 등록한다.
     */
    public OtherPlayer originalOther;//다른 사람이 게임에 들어올 경우 복제되는 오브젝트
    Player player;//나
    //게임안에 있는 사람을 조회하기위한 자료형. Update문이 돌면서 체크하고 값을 넣어줍니다.
    Dictionary<int, OtherPlayer> others = new Dictionary<int, OtherPlayer>();
    //서버에서 받은 좌표정보를 게임에 뿌려서 업데이트 합니다
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
    //사람이 게임에서 나가면 그 사람의 오브젝트를 삭제합니다
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

    private void Start()
    {
        if(GameManager.GetNetwork().IsConnect is false)
        {
            this.enabled = false;
        }
    }
    //Update문이 계속 돌며 서버에서 정보를 받습니다.
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
    //Player(=나)객체가 생성되면 자동으로 이 함수를 호출합니다
    public void AccessRequest(Player _player)
    {
        player =_player;
        print("와1");
    }
    //자신의 정보를 서버에 전송합니다.
    void SendMyDataToServer(Transform t)
    {
        var u = Converting.ToUserTransForm(t.localPosition);
        u.ID = GameManager.Instance.ID;
        GameManager.GetTypeBuff().Push(u);
    }
}

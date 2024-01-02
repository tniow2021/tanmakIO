using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    //UI는 씬별로 따로 스크립트를 만든뒤 게임매니저에 연결한다.
    public void HomeToGame()
    {
        SceneManager.LoadScene(1);
    }
    public void GameToHome()
    {
        SceneManager.LoadScene(0);
    }

    //퍼블릭 설정창 오브젝트
    //설정창 띄우는 버튼
    //설정창 내리는 버튼
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    //UI�� ������ ���� ��ũ��Ʈ�� ����� ���ӸŴ����� �����Ѵ�.

    public TMPro.TMP_InputField inputField;
    public static string IPv4="";
    public void HomeToGame()
    {
        IPv4 = inputField.text;

        SceneManager.LoadScene(1);
    }
    public void GameToHome()
    {
        SceneManager.LoadScene(0);
    }

    //�ۺ� ����â ������Ʈ
    //����â ���� ��ư
    //����â ������ ��ư
}

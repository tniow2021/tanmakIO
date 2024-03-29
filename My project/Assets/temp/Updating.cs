using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Updating : MonoBehaviour
{
    void Awake()
    {
        if(GameManager.Instance is null)
        {
            new GameManager();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.Update();
    }
}

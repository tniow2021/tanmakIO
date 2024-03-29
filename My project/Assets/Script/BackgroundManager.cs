using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject center;
    public GameObject background1;
    public float speedMagnification;//속도 배율
    Vector3 positionMemory;
    SpriteRenderer[] backgroundPool = new SpriteRenderer[4];
    enum site
    {
        leftTop=0,
        rightTop=1,
        leftUnder=2,
        rightUnder=3
    }
    void Start()
    {
        positionMemory = center.transform.position;

        for(int i=0;i<4;i++)
        {
            backgroundPool[i] = Instantiate(background1,center.transform)
                .GetComponent<SpriteRenderer>();
        }
    }

    
    void Update()
    {
        Vector3 variance = new Vector3(0,0,0);
        if (positionMemory !=center.transform.position)
        {
            variance = center.transform.position - positionMemory;
            positionMemory = center.transform.position;


        }
    }

    void Cherk()
    {

    }
}
/*
 * 기능 1. 매 프레임마다 센터오브젝트의 좌표를 확인하고 배경이 필요한 곳에
 * 배경을 복제한다. 배경은 센터오브젝트의 자식으로 들어가며 
 * 카메라의 좌표를 기억해두었다가 센터오브젝트가 움직이면 그에 맞춰 배경을 움직인다.
 * 
 * 
 * 기능 2. 특정좌표에 배경이 필요하다고 함수를 호출하면
 * 배경원본을 복제해서 해당좌표에 복제한다.
 */
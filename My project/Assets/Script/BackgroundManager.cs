using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject center;
    public GameObject background1;
    public float speedMagnification;//�ӵ� ����
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
 * ��� 1. �� �����Ӹ��� ���Ϳ�����Ʈ�� ��ǥ�� Ȯ���ϰ� ����� �ʿ��� ����
 * ����� �����Ѵ�. ����� ���Ϳ�����Ʈ�� �ڽ����� ���� 
 * ī�޶��� ��ǥ�� ����صξ��ٰ� ���Ϳ�����Ʈ�� �����̸� �׿� ���� ����� �����δ�.
 * 
 * 
 * ��� 2. Ư����ǥ�� ����� �ʿ��ϴٰ� �Լ��� ȣ���ϸ�
 * �������� �����ؼ� �ش���ǥ�� �����Ѵ�.
 */
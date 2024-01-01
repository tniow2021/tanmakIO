using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Container : MonoBehaviour
{
    //bullet tag�� ���� ������Ʈ�� ���� �ǰ� ���̰�
    //�ٱ��̸� �����鼭 ������ ������Ʈ ����.
    [Serializable]
    public struct designationForm
    {
        public GameObject originalObj;
        public int number;
    }

    public List<designationForm> objBunch = new List<designationForm>();
    public Rigidbody2D rb;
    public Collider2D cd;
    public int HP = 0;
    public float diffusingFactor = 1;//�������� Ȯ�굵.
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("bullet"))
        {
            HP -= collision.GetComponent<Bullet>().damage;
            if(HP<=0)
            {
                Bang();
            }
        }
    }
    void Bang()
    {
        //������ ����
        foreach(var i in objBunch)
        {
            for(int j=0;j<i.number;j++)
            {
                GameObject clone = Instantiate(i.originalObj);
                clone.transform.position = this.transform.position;
                clone.transform.Translate(UnityEngine.Random.insideUnitCircle* diffusingFactor);

            }
        }
        //�ִϸ��̼� (����)
        //�������ϰ� ������ �������� (����)

        Destroy(this.gameObject);
;    }
}

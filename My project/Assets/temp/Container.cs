using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Container : MonoBehaviour
{
    //bullet tag를 가진 오브젝트에 의해 피가 깎이고
    //다깎이면 터지면서 지정한 오브젝트 생성.
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
    public float diffusingFactor = 1;//아이템의 확산도.
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
        //오브제 생성
        foreach(var i in objBunch)
        {
            for(int j=0;j<i.number;j++)
            {
                GameObject clone = Instantiate(i.originalObj);
                clone.transform.position = this.transform.position;
                clone.transform.Translate(UnityEngine.Random.insideUnitCircle* diffusingFactor);

            }
        }
        //애니메이션 (추후)
        //스무스하게 퍼지는 오브제들 (추후)

        Destroy(this.gameObject);
;    }
}

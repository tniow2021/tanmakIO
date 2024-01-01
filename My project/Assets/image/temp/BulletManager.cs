using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Bulletform
{
    public int number;
    public float angle;
    public float speed;
    public float lifeSpan;
}
public class BulletManager : MonoBehaviour
{
    

    struct waittingBullet
    {
        public Bullet originbullet;
        public Bulletform bulletform;
        public float waittingTime;
        public Transform owner;
    }
    float timer = 0;
    List<waittingBullet> theWaittings = new List<waittingBullet>();
    public bool request(Bulletform bulletform, Bullet originalBullet,float createDelay,Transform owner)//구조체로 대체할 수도
    {
        //안정성 체크
        if (originalBullet is null)
            return false;
        if (bulletform.number <= 0) return false;

        waittingBullet w = new waittingBullet();
        w.originbullet = originalBullet;
        w.bulletform = bulletform;
        w.waittingTime = timer+createDelay;
        w.owner = owner;
        theWaittings.Add(w);
        return true;
    }
    void Start()
    {
        
    }
    void Update()
    {
        timer += Time.deltaTime;

        CreateCherk();
        print(theWaittings.Count);
    }
    void CreateCherk()
    {
        for(int i=0;i<theWaittings.Count;i++)//정렬과 자료구조를 써서 만들어야 최적화된다.
        {
            if(theWaittings[i].waittingTime-timer<=0)
            {
                waittingBullet wb = theWaittings[i];
                Shoot(wb);

                theWaittings.Remove(theWaittings[i]);
            }
        }
    }
    void Shoot(waittingBullet wb)
    {
        Bulletform bf = wb.bulletform;
        Transform owner = wb.owner;
        float lifespan= bf.lifeSpan;
        float angle= bf.angle;
        int number= bf.number;
        float speed= bf.speed;

        for (int i=0;i<number;i++)
        {
            GameObject clone;

            clone = Instantiate(
                wb.originbullet.gameObject,
                owner.position,
                Quaternion.Euler(//부모각도도 더해줌. 상속 시키진 않음.
                        new Vector3(0, 0, owner.rotation.z+ i * 360 / number + angle))
            );
            
           
            Bullet b = clone.GetComponent<Bullet>();
            b.speed= speed;
            b.lifeSpan = lifespan;
        }
        
    }
}

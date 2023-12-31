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
        public Transform parent;
    }
    float timer = 0;
    List<waittingBullet> theWaittings = new List<waittingBullet>();
    public bool request(Bulletform bulletform, Bullet originalBullet,float CreateDelay,Transform parant)//����ü�� ��ü�� ����
    {
        //������ üũ
        if (originalBullet is null)
            return false;
        if (bulletform.number <= 0) return false;

        waittingBullet w = new waittingBullet();
        w.originbullet = originalBullet;
        w.bulletform = bulletform;
        w.waittingTime = timer+CreateDelay;
        w.parent = parant;
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
        for(int i=0;i<theWaittings.Count;i++)//���İ� �ڷᱸ���� �Ἥ ������ ����ȭ�ȴ�.
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
        Transform parent = wb.parent;
        float lifespan= bf.lifeSpan;
        float angle= bf.angle;
        int number= bf.number;
        float speed= bf.speed;

        bool isHaveParent;
        if (wb.parent is not null) isHaveParent = true;
        else isHaveParent = false;

        for (int i=0;i<number;i++)
        {
            GameObject clone;
            if (isHaveParent)//�θ� �������شٸ�
            {
                clone = Instantiate(
                    wb.originbullet.gameObject,
                    parent.position,
                    Quaternion.Euler(//�θ𰢵��� ������
                         new Vector3(0, 0, parent.rotation.z+ i * 360 / number + angle)),
                    parent
                );
            }
            else
            {
                clone = Instantiate(
                    wb.originbullet.gameObject,
                    parent.position,
                    Quaternion.Euler(new Vector3(0, 0, i * 360 / number + angle))
                );
            }
            Bullet b = clone.GetComponent<Bullet>();
            b.speed= speed;
            b.lifeSpan = lifespan;
        }
        
    }
}

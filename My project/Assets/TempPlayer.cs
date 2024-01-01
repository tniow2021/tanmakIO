using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public string UserName = "담영";
    public float moveSpeed = 0;
    public Bullet tempOriginalBullet1;

    public BulletManager bm;
    void Start()
    {
    }
    void Update()
    {
        Move();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            shoot();
        }
    }
    void Move()
    {
        Vector3 frameMoveV3 = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.A)) frameMoveV3.x += -1;
        if (Input.GetKey(KeyCode.D)) frameMoveV3.x += 1;
        if (Input.GetKey(KeyCode.S)) frameMoveV3.y += -1;
        if (Input.GetKey(KeyCode.W)) frameMoveV3.y += 1;

        transform.Translate(frameMoveV3 * Time.deltaTime * moveSpeed);
    }
    void shoot()
    {
        Bulletform bulletForm = new Bulletform();
        for(int i=0;i<30;i++)
        {
            bulletForm.number = 1;
            bulletForm.speed = 20;
            bulletForm.lifeSpan = 5;
            bulletForm.angle = i*12;
            bool Iscomplite =
                bm.request(bulletForm, tempOriginalBullet1, i*0.05f, transform);
            if (Iscomplite is false) 디버깅.불렛매니저의리퀘스트함수가실패함();
        }
    }
}

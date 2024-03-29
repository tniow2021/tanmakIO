using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public BulletManager bulletManager;
    public Bullet originBullet;
    void Start()
    {
        
    }
    void Update()
    {
        Move();
        Shooting();
    }
    void Move()
    {
        Vector3 moveV3 = new Vector3();
        if (Input.GetKey(KeyCode.A)) moveV3.x -= 1;
        if (Input.GetKey(KeyCode.D)) moveV3.x += 1;
        if (Input.GetKey(KeyCode.W)) moveV3.y += 1;
        if (Input.GetKey(KeyCode.S)) moveV3.y -= 1;
        transform.Translate(moveV3 * speed * Time.deltaTime);

        Vector3 rotateV3 = new Vector3();
        if (Input.GetMouseButton(0)) rotateV3.z += 1;
        if (Input.GetMouseButton(1)) rotateV3.z -= 1;

        transform.Rotate(rotateV3 * rotateSpeed * Time.deltaTime);
    }
    void Shooting()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Bulletform bf = new Bulletform();
            bf.angle = 0;
            bf.lifeSpan = 3;
            bf.number = 1;
            bf.speed = 20;
            bulletManager.request(bf, originBullet, 0, transform);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("∏‘¿Ã"))
        {
            Destroy(collision.gameObject);
        }
    }
}

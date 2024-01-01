using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=0;
    public int delayTime=0;
    public float lifeSpan = 0;
    public int damage = 0;
    private void Awake()
    {
    }
    void Start()
    {
        this.transform.tag = "bullet";
    }

    float timer = 0;
    //int cherkHelper = 0;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifeSpan) Die();

        transform.Translate(0,speed * Time.deltaTime,0);
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
}

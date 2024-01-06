using UnityEngine;

public class Player : MonoBehaviour
{
    public string UserName = "�㿵";
    public float moveSpeed = 0;
    public Bullet tempOriginalBullet1;
    public BulletManager bm;
    void Start()
    {
        //�ֻ��� ������Ʈ�� �޷��ִ� InGameNet�� ã�� ���
        transform.root.GetComponent<InGameNet>().AccessRequest(this);
        print("player");
    }
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
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
        for (int i = 0; i < 30; i++)
        {
            bulletForm.number = 1;
            bulletForm.speed = 20;
            bulletForm.lifeSpan = 5;
            bulletForm.angle = i * 12;
            bool Iscomplite =
                bm.request(bulletForm, tempOriginalBullet1, i * 0.05f, transform);
            if (Iscomplite is false) �����.�ҷ��Ŵ����Ǹ�����Ʈ�Լ���������();
        }
    }
}
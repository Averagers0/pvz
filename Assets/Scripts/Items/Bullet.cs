using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;      // 子弹速度
    public int damage = 20;       // 子弹伤害
    public float lifeTime = 5f;   // 存活时间（避免无限存在）

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Zombie zombie = collision.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);
            Destroy(gameObject); // 命中僵尸后销毁子弹
        }
    }
}

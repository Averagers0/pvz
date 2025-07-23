using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowPeaBullet : Bullet
{
    public float slowDuration = 2f;     // 减速持续时间
    public float slowFactor = 0.5f;     // 减速比例（0.5 = 50%速度）

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Zombie zombie = collision.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage);
            zombie.SlowDown(slowDuration, slowFactor); // 减速逻辑
            Destroy(gameObject); // 命中僵尸后销毁子弹
        }
    }
}


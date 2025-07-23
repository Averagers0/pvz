using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peashooter : Plant
{
    [Header("Peashooter Settings")]
    public GameObject bulletPrefab;    // 豌豆子弹预制体
    public Transform firePoint;        // 子弹发射位置
    public float fireRate = 1.5f;      // 每多少秒发射一次

    private Coroutine fireCoroutine;
    private void Awake()
    {
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");

            if (firePoint == null)
            {
                Debug.Log("找不到");
                firePoint = transform; // 找不到就用自己位置
            }
        }
    }

    private void OnEnable()
    {
        // 当植物激活时开始射击
        fireCoroutine = StartCoroutine(FireLoop());
    }

    private void OnDisable()
    {
        // 植物被移除时停止射击
        if (fireCoroutine != null)
            StopCoroutine(fireCoroutine);
    }

    public override void SpecialAbility()
    {
        // Peashooter 的特殊能力：发射子弹（逻辑已在 FireLoop 内实现）
    }

    /// <summary>
    /// 按固定时间间隔发射子弹
    /// </summary>
    private IEnumerator FireLoop()
    {
        while (true)
        {
            if (HasZombieInFront())
            {
                Shoot();
                yield return new WaitForSeconds(fireRate);
            }
            else
            {
                yield return new WaitForSeconds(0.2f); // 没僵尸时更快检查
            }
        }
    }

    private bool HasZombieInFront()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (var z in zombies)
        {
            // 检查是否在同一行
            if (Mathf.Abs(z.transform.position.y - transform.position.y) < 0.8f)
            {
                // 检查是否在豌豆射手前方（右侧）
                if (z.transform.position.x > transform.position.x)
                    return true;
            }
        }
        return false;
    }



    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}


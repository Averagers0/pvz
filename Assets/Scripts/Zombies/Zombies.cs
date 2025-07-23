using System.Collections;
using UnityEngine;

public abstract class Zombie : MonoBehaviour
{
    public ZombieData zombieData;
    private int currentHealth;

    public float moveSpeed;

    public int attackDamage;
    public float attackInterval;

    private SpriteRenderer spriteRenderer;
    private bool isSlowed = false;
    private Coroutine currentSlowCoroutine;


    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = zombieData.health;
        moveSpeed = zombieData.originalSpeed;
        attackDamage = zombieData.originalDamage;
        attackInterval = zombieData.originalInterval;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void SlowDown(float duration, float slowFactor)
    {
        // 如果已经处于减速状态，就只刷新时间，不再次叠加
        if (isSlowed)
        {
            // 重置协程
            StopCoroutine(currentSlowCoroutine);
            currentSlowCoroutine = StartCoroutine(SlowCoroutine(duration));
        }
        else
        {
            // 第一次减速
            moveSpeed = zombieData.originalSpeed * slowFactor;
            if (spriteRenderer != null)
                spriteRenderer.color = Color.blue;

            isSlowed = true;
            currentSlowCoroutine = StartCoroutine(SlowCoroutine(duration));
        }
    }

    private IEnumerator SlowCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        // 恢复速度
        moveSpeed = zombieData.originalSpeed;
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        isSlowed = false;
    }


    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}


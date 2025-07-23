using System.Collections;
using UnityEngine;

public class NormalZombie : Zombie
{
    [Header("Zombie Settings")]
    public Animator animator;            // 动画控制器
    public float sameRowTolerance = 0.8f; // 判断是否同一行的y坐标容差

    private bool isWalk = false;
    private bool isDead = false;
    private bool isAttack = false;

    private Rigidbody2D rb;
    private Plant targetPlant;           // 当前攻击的植物
    private Coroutine attackCoroutine;   // 攻击协程引用

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead && !isAttack)
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        isWalk = true;
        animator.SetBool("isWalk", isWalk);
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
        if (isDead) return;
        base.TakeDamage(damage);
    }


    protected override void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plant plant = collision.GetComponent<Plant>();
        // 只攻击同一行的植物
        if (plant != null && !isDead && IsSameRow(plant))
        {
            targetPlant = plant;
            StartAttack();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Plant>() == targetPlant)
        {
            StopAttack();
        }
    }

    private void StartAttack()
    {
        isAttack = true;
        isWalk = false;
        animator.SetBool("isWalk", false);
        animator.SetBool("isAttack", true);
        attackCoroutine = StartCoroutine(AttackLoop());
    }

    private void StopAttack()
    {
        isAttack = false;
        isWalk = true;
        animator.SetBool("isWalk", true);
        animator.SetBool("isAttack", false);
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        targetPlant = null;
    }

    private IEnumerator AttackLoop()
    {
        while (targetPlant != null && !isDead)
        {
            targetPlant.TakeDamage(attackDamage);
            // 如果植物被销毁（死亡），立即停止攻击
            if (targetPlant == null)
            {
                StopAttack();
                yield break;
            }
            yield return new WaitForSeconds(attackInterval);
        }
        StopAttack(); // 没有植物时恢复移动
    }

    // 判断是否同一行
    private bool IsSameRow(Plant plant)
    {
        SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();
        SpriteRenderer plantRenderer = plant.GetComponent<SpriteRenderer>();

        return plantRenderer != null &&
               myRenderer.sortingOrder == plantRenderer.sortingOrder;
    }

}

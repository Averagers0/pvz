using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{
    private Animator animator;

    // 血量阈值
    public int crack1Threshold = 150;  // 例如：低于 150 播放 Crack1
    public int crack2Threshold = 50;   // 例如：低于 50 播放 Crack2

    private bool isCrack1Played = false;
    private bool isCrack2Played = false;

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void SpecialAbility()
    {
        Debug.Log("坚果种植");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage); // 先减血并检查死亡

        // 检查血量并播放对应动画
        if (!isCrack1Played && currentHealth <= crack1Threshold)
        {
            isCrack1Played = true;
            animator.SetBool("isCrack1Played", isCrack1Played);
        }
        if (!isCrack2Played && currentHealth <= crack2Threshold)
        {
            isCrack2Played = true;
            animator.SetBool("isCrack2Played", isCrack2Played);
        }
    }
}


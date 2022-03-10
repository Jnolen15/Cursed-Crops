using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarrotAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private ScarrotAttack sa;
    private EnemyToPlayer etp;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        sa = this.GetComponentInParent<ScarrotAttack>();
        etp = this.GetComponentInParent<EnemyToPlayer>();
    }

    void Update()
    {
        // Update speed
        animator.SetFloat("Speed", etp.enemySpeed);

        // Attacking
        animator.SetBool("Attacking", sa.attacking);
        animator.SetBool("OnCooldown", sa.onCooldown);

        // AOE Attack
        animator.SetBool("AOE", sa.attackAOE);

        // Punch/Dash Attack
        animator.SetBool("Punch", sa.attackDash);
    }

    public void DashAttack()
    {
        sa.DashAttack();
    }

    public void AOEAttack()
    {
        sa.AOEAttack();
    }
}

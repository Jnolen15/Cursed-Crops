using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarrotAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private ScarrotAttack sa;
    private EnemyToPlayer etp;
    private SpriteRenderer sr;

    private Vector3 prev;
    private Vector3 current;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        sa = this.GetComponentInParent<ScarrotAttack>();
        etp = this.GetComponentInParent<EnemyToPlayer>();
        sr = this.GetComponent<SpriteRenderer>();

        prev = this.transform.parent.position;
    }

    void Update()
    {
        // Flip based on direction
        current = this.transform.parent.position;
        if (prev != current)
        {
            Vector3 temp = (current - prev).normalized;
            if (temp.x > 0)
            {
                sr.flipX = false;
            }
            else if (temp.x < 0)
            {
                sr.flipX = true;
            }
            prev = current;
        }

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

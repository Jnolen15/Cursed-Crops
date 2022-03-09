using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private SpriteRenderer sr;
    private Animator animator;
    private RangeEnemy re;
    private EnemyControler ec;
    public bool shot = false;
    private bool flipped = false;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        re = this.GetComponentInParent<RangeEnemy>();
        ec = this.GetComponentInParent<EnemyControler>();
    }

    void Update()
    {
        if (ec.stunned)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("Shooting", false);
        }
        else
        {
            animator.SetFloat("Speed", re.enemySpeed);
            animator.SetBool("Shooting", re.shooting);
        }

        // Face the direction they are shooting
        if (re.direction.x > 0 && flipped)
        {
            flipped = false;
            sr.flipX = false;
        }
        else if (re.direction.x < 0 && !flipped)
        {
            flipped = true;
            sr.flipX = true;
        }
    }

    void Shoot()
    {
        re.Shoot();
    }
}

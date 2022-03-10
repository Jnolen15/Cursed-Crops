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

    private Vector3 prev;
    private Vector3 current;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
        re = this.GetComponentInParent<RangeEnemy>();
        ec = this.GetComponentInParent<EnemyControler>();

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

        // Face the direction they are shooting
        /*if (re.direction.x > 0 && flipped)
        {
            flipped = false;
            sr.flipX = false;
        }
        else if (re.direction.x < 0 && !flipped)
        {
            flipped = true;
            sr.flipX = true;
        }*/

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
    }

    void Shoot()
    {
        re.Shoot();
    }
}

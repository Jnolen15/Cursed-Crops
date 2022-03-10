using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediberryAnimator : MonoBehaviour
{
    // ================= Public variables =================


    // ================= Private variables =================
    private Animator animator;
    private GoToEnemy gTE;
    public bool spawning = true;
    private SpriteRenderer sr;

    private Vector3 prev;
    private Vector3 current;


    void Start()
    {
        animator = this.GetComponent<Animator>();
        gTE = this.GetComponentInParent<GoToEnemy>();
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
        animator.SetFloat("Speed", gTE.enemySpeed);
    }
}

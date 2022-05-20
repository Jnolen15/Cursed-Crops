using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SabomatoAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private SpriteRenderer sr;
    private SaboAI sai;

    private Vector3 prev;
    private Vector3 current;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        sai = this.GetComponentInParent<SaboAI>();

        prev = this.transform.parent.position;
    }

    // Update is called once per frame
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
        animator.SetFloat("Speed", sai.enemySpeed);

        // Updating Attack
        animator.SetBool("Attacking", sai.sabotaging);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornonAnimation : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private CornnonAI cAI;
    private SpriteRenderer sr;

    private Vector3 prev;
    private Vector3 current;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        cAI = this.GetComponentInParent<CornnonAI>();
        sr = this.GetComponent<SpriteRenderer>();

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
        animator.SetFloat("Speed", cAI.enemySpeed);

        // Shooting
        animator.SetBool("Shooting", cAI.shooting);
    }

    void EndSpawning()
    {
        cAI.spawning = false;
    }

    void Shoot()
    {
        cAI.Shoot();
    }

    void Die()
    {
        cAI.Die();
    }
}

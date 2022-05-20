using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrocLiAnimation : MonoBehaviour
{
    // ================= Public variables =================


    // ================= Private variables =================
    private Animator animator;
    private EnemyToPlayer etp;
    private WindupWithStun wus;
    private EnemyControler ec;
    private SpriteRenderer sr;

    private Vector3 prev;
    private Vector3 current;
    private float timeSinceDirSwitch;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        etp = this.gameObject.GetComponentInParent<EnemyToPlayer>();
        wus = this.gameObject.GetComponentInParent<WindupWithStun>();
        ec = this.gameObject.GetComponentInParent<EnemyControler>();
        sr = this.GetComponent<SpriteRenderer>();

        prev = this.transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Flip based on direction
        if (timeSinceDirSwitch < 0.2f) timeSinceDirSwitch += Time.deltaTime;
        else 
        {
            current = this.transform.parent.position;
            if (prev != current)
            {
                Vector3 temp = (current - prev).normalized;
                if (temp.x > 0)
                {
                    sr.flipX = false;
                    timeSinceDirSwitch = 0;
                }
                else if (temp.x < 0)
                {
                    sr.flipX = true;
                    timeSinceDirSwitch = 0;
                }
                prev = current;
            }
        }

        if (ec.stunned)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("Attacking", false);
        } else
        {
            animator.SetFloat("Speed", etp.enemySpeed);
            animator.SetBool("Attacking", wus.attacking);
        }
    }
}

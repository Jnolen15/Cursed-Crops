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

    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponent<Animator>();
        etp = this.gameObject.GetComponentInParent<EnemyToPlayer>();
        wus = this.gameObject.GetComponentInParent<WindupWithStun>();
        ec = this.gameObject.GetComponentInParent<EnemyControler>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ec.stunned)
        {
            animator.SetFloat("Speed", 0);
            animator.SetBool("Attacking", false);
        } else
        {
            animator.SetFloat("Speed", etp.enemySpeed);
            animator.SetBool("Attacking", wus.attacking);
        }
        
        /*if (wus.attacking)
        {
            animator.SetTrigger("Attack");
        }*/
    }
}

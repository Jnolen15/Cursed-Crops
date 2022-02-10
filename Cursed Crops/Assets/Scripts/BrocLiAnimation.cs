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

    // Start is called before the first frame update
    void Start()
    {
        animator = this.transform.GetChild(0).GetComponent<Animator>();
        etp = this.gameObject.GetComponent<EnemyToPlayer>();
        wus = this.gameObject.GetComponent<WindupWithStun>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", etp.enemySpeed);
        if (wus.stunned)
        {
            animator.SetBool("Attacking", false);
        } else
        {
            animator.SetBool("Attacking", wus.attacking);
        }
        
        /*if (wus.attacking)
        {
            animator.SetTrigger("Attack");
        }*/
    }
}

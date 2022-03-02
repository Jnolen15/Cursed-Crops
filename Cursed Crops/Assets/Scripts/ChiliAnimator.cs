using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiliAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private RangeEnemy rE;
    public bool shot = false;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        rE = this.GetComponentInParent<RangeEnemy>();
    }

    void Update()
    {
        // Update speed
        animator.SetFloat("Speed", rE.enemySpeed);

        // Shooting
        animator.SetBool("Shooting", rE.shooting);
        /*if (rE.shooting && !shot)
        {
            shot = true;
            animator.SetTrigger("Shoot");
        } else if (!rE.shooting && shot)
        {
            shot = false;
        }*/
    }

    void Shoot()
    {
        rE.StartCoroutine("shoot"); 
    }
}

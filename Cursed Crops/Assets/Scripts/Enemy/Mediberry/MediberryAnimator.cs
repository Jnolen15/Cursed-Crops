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


    void Start()
    {
        animator = this.GetComponent<Animator>();
        gTE = this.GetComponentInParent<GoToEnemy>();
    }

    void Update()
    {
        // Update speed
        animator.SetFloat("Speed", gTE.enemySpeed);
    }
}

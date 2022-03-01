using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageAnimator : MonoBehaviour
{
    // ================= Public variables =================


    // ================= Private variables =================
    private Animator animator;
    private GrabbageAI gAI;
    private GrabbageWindup gW;
    private EnemyToPlayer eTP;
    public bool spawning = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        gAI = this.GetComponentInParent<GrabbageAI>();
        gW = this.GetComponentInParent<GrabbageWindup>();
        eTP = this.GetComponentInParent<EnemyToPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Don't move if spawnning
        if (spawning) eTP.enemySpeed = 0;
        else
        {
            eTP.enemySpeed = eTP.originalSpeed;
            animator.SetBool("Spawning", false);
        }

        // Update speed
        animator.SetFloat("Speed", eTP.enemySpeed);

        // Update Grab
        animator.SetBool("Grab", gW.windupStarting);

        // Updating Attack
        animator.SetBool("Attacking", gAI.alreadyGrabbing);
    }

    void EndSpawning()
    {
        spawning = false;
    }
}

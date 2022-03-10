using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbageAnimator : MonoBehaviour
{
    // ================= Public variables =================
    public bool spawning = true;

    // ================= Private variables =================
    private Animator animator;
    private GrabbageAI gAI;
    private GrabbageWindup gW;
    private GrabbageToPlayers gTP;
    private SpriteRenderer sr;

    private Vector3 prev;
    private Vector3 current;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        gAI = this.GetComponentInParent<GrabbageAI>();
        gW = this.GetComponentInParent<GrabbageWindup>();
        gTP = this.GetComponentInParent<GrabbageToPlayers>();
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

        // Don't move if spawnning
        if (spawning) gTP.enemySpeed = 0;
        else
        {
            gTP.enemySpeed = gTP.originalSpeed;
            animator.SetBool("Spawning", false);
        }

        // Update speed
        animator.SetFloat("Speed", gTP.enemySpeed);

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornonAnimation : MonoBehaviour
{
    // ================= Public variables =================


    // ================= Private variables =================
    private Animator animator;
    private CornnonAI cAI;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        cAI = this.GetComponentInParent<CornnonAI>();
    }

    // Update is called once per frame
    void Update()
    {
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

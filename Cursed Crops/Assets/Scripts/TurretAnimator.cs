using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAnimator : MonoBehaviour
{
    public Animator animator;
    public Turret turret;

    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        turret = this.GetComponentInParent<Turret>();
    }

    public void playShoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void shoot()
    {
        turret.MakeBullet();
    }
}

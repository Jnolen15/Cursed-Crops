using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private AudioPlayer daSound;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        //daSound = this.GetComponentInParent<AudioPlayer>();
    }

    // Animation triggered functions
    public void Shoot() {  }

    public void AttackLunge() {  }

    public void AttackHit() {  }

    public void AttackEnd() {  }

    public void AttackCancled() {  }

    public void StopMovement() {  }

    public void StartMovement() {  }

    // Animation triggered sounds
    public void PlayWalkSound() { /*daSound.PlaySound(pc.stepSound);*/ }
}

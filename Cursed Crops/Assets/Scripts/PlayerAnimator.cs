using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // ================= Private variables =================
    private Animator animator;
    private PlayerControler pc;
    private AudioPlayer daSound;

    /* Most all animations are triggered from the PlayerController script
     * This script is used as a helper to trigger functions to be
     * correctly timed using animation events. This cannot be done from
     * the PC script as it is not attached the the animated object.
     */
    void Start()
    {
        animator = this.GetComponent<Animator>();
        pc = this.GetComponentInParent<PlayerControler>();
        daSound = this.GetComponentInParent<AudioPlayer>();
    }

    void Update()
    {

    }

    // Animation triggered functions
    public void Shoot() { pc.Shoot(); }

    public void AttackLunge() { pc.AttackLunge(); }

    public void AttackHit() { pc.DoAttack(); }

    public void AttackEnd() { pc.AttackEnd(); }

    public void AttackCancled() { pc.AttackCancled(); }

    public void StopMovement() { pc.stopMovement = true; }

    public void StartMovement() { pc.stopMovement = false; }

    // Animation triggered sounds
    public void PlayWalkSound() { daSound.PlaySound(pc.stepSound); }

}

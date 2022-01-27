using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimOCManager : MonoBehaviour
{
    // --- Variables
    public AnimatorOverrideController harveyAnimOC;
    public AnimatorOverrideController dougAnimOC;
    public AnimatorOverrideController cecilAnimOC;
    public character selectedCharacter;
    private Animator animator;

    // --- Enums
    public enum character // your custom enumeration
    {
        harvey,
        doug,
        cecil
    };

    // --- Variables
    void Start()
    {
        animator = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>();
        if (selectedCharacter == character.harvey)
        {
            animator.runtimeAnimatorController = harveyAnimOC;
        }
        else if (selectedCharacter == character.doug)
        {
            animator.runtimeAnimatorController = dougAnimOC;
        }
        else if (selectedCharacter == character.cecil)
        {
            animator.runtimeAnimatorController = cecilAnimOC;
        }
    }
}

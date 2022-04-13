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
        Harvey,
        Doug,
        Cecil, 
        Carlisle
    };

    // --- Variables
    void Start()
    {
        animator = this.GetComponent<Animator>();
        if (selectedCharacter == character.Harvey)
        {
            animator.runtimeAnimatorController = harveyAnimOC;
        }
        else if (selectedCharacter == character.Doug)
        {
            animator.runtimeAnimatorController = dougAnimOC;
        }
        else if (selectedCharacter == character.Cecil)
        {
            animator.runtimeAnimatorController = cecilAnimOC;
        }
    }
}

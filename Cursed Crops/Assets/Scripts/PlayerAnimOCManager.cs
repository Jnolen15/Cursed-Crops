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
        else if (selectedCharacter == character.Carlisle)
        {
            animator.runtimeAnimatorController = harveyAnimOC;
        }
    }

    public void SetCharacter(string chara)
    {
        Debug.Log("Got string: " + chara);
        switch(chara)
        {
            case "Cecil":
                selectedCharacter = character.Cecil;
                break;
            case "Doug":
                selectedCharacter = character.Doug;
                break;
            case "Harvey":
                selectedCharacter = character.Harvey;
                break;
            case "Carlisle":
                selectedCharacter = character.Carlisle;
                break;
        }
    }
}

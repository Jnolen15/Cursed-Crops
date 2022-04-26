using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueClass
{
    public bool endOfDialogue;
    public bool enemyAttack;
    public bool endOfStartingDialogue;
    public string name;
    public string eventWeWant;

    [TextArea(4, 10)]
    public string sentences;
}

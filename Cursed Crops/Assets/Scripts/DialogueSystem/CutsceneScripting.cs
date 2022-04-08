using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneScripting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (gameObject.GetComponent<DialogueTrigger>().dialogueHappening && Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameObject.GetComponent<DialogueTrigger>().stopDialogue)
            {
                gameObject.GetComponent<DialogueTrigger>().DisplayNextSentence();
            }
            else
            {
                gameObject.GetComponent<DialogueTrigger>().Resume();
            }
        }*/
    }

    public void Progress_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            if (gameObject.GetComponent<DialogueTrigger>().dialogueHappening)
            {
                if (!gameObject.GetComponent<DialogueTrigger>().stopDialogue)
                {
                    gameObject.GetComponent<DialogueTrigger>().DisplayNextSentence();
                }
                else
                {
                    gameObject.GetComponent<DialogueTrigger>().Resume();
                }
            }
        }
    }
}

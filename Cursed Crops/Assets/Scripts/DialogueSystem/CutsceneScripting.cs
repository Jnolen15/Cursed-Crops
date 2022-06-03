using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneScripting : MonoBehaviour
{
    public DialogueTrigger dt;
    public string goToLevel;

    // Start is called before the first frame update
    void Start()
    {
        dt = this.GetComponent<DialogueTrigger>();
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
        dt.CutsceneOver(goToLevel);
    }

    public void Progress_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            
            if (gameObject.GetComponent<DialogueTrigger>().dialogueHappening)
            {
                if (dt.textOver)
                {
                    if (!gameObject.GetComponent<DialogueTrigger>().stopDialogue)
                    {
           

                        gameObject.GetComponent<DialogueTrigger>().DisplayNextSentence();
                    }
                    else
                    {
                        gameObject.GetComponent<DialogueTrigger>().Resume();
                    }
                } else
                {
                    // Skip text animation
                    dt.SkipAnimation();
                }

            }
        }
    }

    public void Skip_performed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.performed)
        {
            dt.SkipCutscene(goToLevel);
        }
    }
}

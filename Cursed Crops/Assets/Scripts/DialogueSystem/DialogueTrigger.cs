using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{

    //Getting the cutscene manager
    public PlayableDirector director;

    //Variables that will change the character picture, name, etc
    public GameObject daName;
    public GameObject image;
    public GameObject daDialogue;

    public Sprite Doug;
    public Sprite Cecil;
    public Sprite Carlisle;
    public Sprite Harvey;
    public Sprite Narrator;
    public Sprite Cultist;



    //Variables for calling the Dialogue Class
    public string sentence;
    public string character;
    public bool stopDialogue;
    public bool dialogueHappening;
    public bool startOfDialogue = true;
    public DialogueClass[] dialogue;


    private Queue<string> sentences;
    private Queue<string> characters;
    private Queue<bool> stoppers;
    

    void Start()
    {
        sentences = new Queue<string>();
        characters = new Queue<string>();
        stoppers = new Queue<bool>();
    }
    public void TriggerDialogue()
    {
        Pause();

        if (startOfDialogue)
        {
            startOfDialogue = false;
            sentences.Clear();
            characters.Clear();
            stoppers.Clear();

            // Goes through all the lines of dialogue with who is speaking
            // their line and if they end the dialogue sequence
            foreach (DialogueClass line in dialogue)
            {


                //sentences.Enqueue(sentence);

                stoppers.Enqueue(line.endOfDialogue);
                characters.Enqueue(line.name);
                sentences.Enqueue(line.sentences);
                //line.endOfDialogue = false;

            }
        }
            DisplayNextSentence();
        

    }

    public void DisplayNextSentence()
    {
        
        if (sentences.Count == 0 || characters.Count == 0)
        {
            startOfDialogue = true;
            Resume();
            return;
        }
        stopDialogue = stoppers.Dequeue();
        Debug.Log(stopDialogue);

        daName.GetComponent<TextMeshProUGUI>().text = characters.Dequeue();
        
        //Deals with the images of the dialogue
        switch (daName.GetComponent<TextMeshProUGUI>().text)
        {

            case "Narrator":
                image.GetComponent<Image>().sprite = Narrator;
                break;
            case "Cecil":
                image.GetComponent<Image>().sprite = Cecil;
                break;
            case "Doug":
                image.GetComponent<Image>().sprite = Doug;
                break;
            case "Carlisle":
                image.GetComponent<Image>().sprite = Carlisle;
                break;
            case "Harvey":
                image.GetComponent<Image>().sprite = Harvey;
                break;
            case "Cultist":
                image.GetComponent<Image>().sprite = Cultist;
                break;

        }


        daDialogue.GetComponent<TextMeshProUGUI>().text = sentences.Dequeue();

        //Use this space to add the letter animations - Juan
        
    }

    void Pause()
    {
        dialogueHappening = true;
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void Resume()
    {
        dialogueHappening = false;
        
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrigger : MonoBehaviour
{
    public PlayableDirector director;
    public string sentence;

    public void TriggerDialogue()
    {
        Pause();
       
        Debug.Log(sentence);
        
    }

    void Pause()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void Resume()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
}

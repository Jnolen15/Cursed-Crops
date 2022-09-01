using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSoundDelay : MonoBehaviour
{
    // Start is called before the first frame update
    // Pointer on level Sound
    public AudioClip pointerOnSound;
    public AudioClip pointerOffSound;
    public AudioSource buttonAudio;


    private bool canPlaySound = false;
    private bool waitFortime = false;
    private bool playSoundOnce = false;
    private float counter = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(counter);
        if (waitFortime)
        {
            counter += Time.deltaTime;
        }
        if (counter >= 0.1f)
        {
            canPlaySound = true;
            if (!playSoundOnce)
            {
                playSoundOnce = true;
                buttonAudio.PlayOneShot(pointerOnSound);
            }
        }
    }

    public void pointerOnLevelButton()
    {
        waitFortime = true;
        
    }

    public void pointerOffLevelButton()
    {
        
        if (canPlaySound)
        {
            canPlaySound = false;
            buttonAudio.PlayOneShot(pointerOffSound);
        }
        waitFortime = false;
        counter = 0;
        playSoundOnce = false;
    }
}

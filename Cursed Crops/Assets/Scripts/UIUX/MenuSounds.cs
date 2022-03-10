using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip mouseOverSound;
    public AudioClip mouseExitSound;
    public AudioClip mouseEnterSound;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        Debug.Log(gameObject);
        gameObject.GetComponent<AudioPlayer>().PlaySound(mouseOverSound);
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<AudioPlayer>().PlaySound(mouseExitSound);
    }

    public void OnMouseDown()
    {
        gameObject.GetComponent<AudioPlayer>().PlaySound(mouseEnterSound);
    }
}

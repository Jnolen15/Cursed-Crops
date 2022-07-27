using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    private AudioSource ASource;
    private PlayerManager PManager;
    private float InitialVolume;

    // where sound falls off completely
    private float MaxDistance = 50;
    // distance where sound starts falling off
    private float MinDistance = 5;

    private bool ScriptActive = false;

    // Start is called before the first frame update
    void Start()
    {
        ASource = GetComponent<AudioSource>();
        PManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        InitialVolume = ASource.volume;
    }

    // think a lot of this computing could be simplified if distance to the closest player was calculated
    // in the PlayerManager, and then simply used once by this script, which is on every audio 
    // object

    // Update is called once per frame
    void Update()
    {
        if (ScriptActive)
        {
            return;
        }
        // calculate distance to closest player
        float playerDistance = MaxDistance;
        foreach (GameObject player in PManager.players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if ( distance < playerDistance)
            {
                playerDistance = distance;
            }
        }

        // max volume; scaled volume; min volume based on distance to closest player
        if (playerDistance < MinDistance)
        {
            ASource.volume = InitialVolume;
        } else if (playerDistance < MaxDistance)
        {
            ASource.volume = InitialVolume * (1 - ((playerDistance - MinDistance) / (MaxDistance - MinDistance)));
        } else
        {
            ASource.volume = 0;
        }
        Debug.Log("Setting Volume to: " + (1 - ((playerDistance - MinDistance) / (MaxDistance - MinDistance))) + "%");
    }
}

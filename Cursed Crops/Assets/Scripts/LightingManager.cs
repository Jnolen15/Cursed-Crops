using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Color MorningLight;
    public Color AfternoonLight;
    public Color NightLight;

    private float time = 0;
    private float AngleTimer = 0;
    private int LightPhase = 1;
    private Quaternion StartRotation;

    public Light LightSource;

    void Start()
    {
        this.StartRotation = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (LightPhase)
        {
            case 1: 
                ColorFade(MorningLight, AfternoonLight, 10);
                break;
            case 2:
                ColorFade(AfternoonLight, NightLight, 10);
                break;
        }
        AngleFade(0, 90, 20); 
    }

    public void ResetTimer()
    {
        time = 0;
    }

    public void ColorFade(Color startC, Color endC, float t) {
        if (time < 1)
        {
            time += Time.deltaTime / t;
            LightSource.color = Color.Lerp(startC, endC, time);
            // Debug.Log("Changing Light: " + time);
        } else
        {
            LightPhase += 1;
            ResetTimer();
        }
    }

    public void AngleFade(float startA, float endA, float t)
    {
        if (AngleTimer < 1)
        {
            AngleTimer += Time.deltaTime / t;
            this.gameObject.transform.rotation = StartRotation * Quaternion.Euler(0, Mathf.Lerp(startA, endA, AngleTimer), 0);
            Debug.Log("Changing Agnle: " + AngleTimer);
        }

    }
}

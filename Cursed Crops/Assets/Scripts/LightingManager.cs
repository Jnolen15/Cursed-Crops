using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Color MorningLight;
    public Color AfternoonLight;
    public Color NightLight;

    private float time = 0;
    private int LightPhase = 1;

    public Light LightSource;

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
            case 3:
                ColorFade(NightLight, MorningLight, 10);
                break;
        }
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
        // just gonna have this piggy back off of colorFade, will not work on its own
        this.gameObject.transform.rotation *= Quaternion.Euler(0, Mathf.Lerp(startA, endA, time), 0);
    }
}

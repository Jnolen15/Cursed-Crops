using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Color MorningLight;
    public Color AfternoonLight;
    public Color NightLight;

    public float MorningAngle = 10;
    public float AfternoonAngle = 70;
    public float NightAngle = 10;

    public float MorningOffest = 100;
    public float AfternoonOffset = 180;
    public float NightOffset = 260;

    public float PhaseTime = 10;

    private float time = 0;
    private float AngleTimer = 0;
    private int LightPhase = 1;
    private int AnglePhase = 1;
    private Quaternion StartRotation;

    public Light LightSource;

    void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(MorningAngle, MorningOffest, 0);
        this.StartRotation = this.gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch (LightPhase)
        {
            case 1: 
                ColorFade(MorningLight, AfternoonLight, PhaseTime);
                break;
            case 2:
                ColorFade(AfternoonLight, NightLight, PhaseTime);
                break;
        }

        switch (AnglePhase) {
            case 1:
                AngleFade(0, AfternoonAngle - MorningAngle,
                    0, AfternoonOffset - MorningOffest,
                    PhaseTime);
                break;

            case 2:
                AngleFade(AfternoonAngle - MorningAngle, 0,
                    AfternoonOffset - MorningOffest, NightOffset - MorningOffest,
                    PhaseTime);
                break;
        }
        // AngleFade(0, 120, 20); 
    }

    public void ColorFade(Color startC, Color endC, float t) {
        if (time < 1)
        {
            time += Time.deltaTime / t;
            LightSource.color = Color.Lerp(startC, endC, time);
            // Debug.Log("Changing Light: " + time);
        } else
        {
            LightPhase++;
            time = 0;
        }
    }

    // lerps the lights X angle from start to b, Y angle from b to c, over t (time), 
    public void AngleFade(float startX, float endX, float startY, float endY, float t)
    {
        if (AngleTimer < 1)
        {
            AngleTimer += Time.deltaTime / t;
            this.gameObject.transform.rotation = Quaternion.Euler(MorningAngle + Mathf.Lerp(startX, endX, AngleTimer),
                                                                  MorningOffest + Mathf.Lerp(startY, endY, AngleTimer),
                                                                  0);

            // Debug.Log("Changing X Angle: " + AngleTimer + " by " + Mathf.Lerp(startX, endX, AngleTimer) + " degrees; " + startX + ", " + endX);
            // Debug.Log("Changing Y Angle: " + AngleTimer + " by " + Mathf.Lerp(startY, endY, AngleTimer) + " degrees; " + startY + ", " + endY);
        } else
        {
            AnglePhase++;
            AngleTimer = 0;
        }
    }
}

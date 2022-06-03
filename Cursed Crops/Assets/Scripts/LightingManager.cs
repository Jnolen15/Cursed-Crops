using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Color DawnLight;
    public Color MorningLight;
    public Color AfternoonLight;
    public Color DuskLight;
    public Color NightLight;

    public float MorningAngle = 10;
    public float AfternoonAngle = 70;
    private float NightAngle = 10;

    public float MorningOffest = 100;
    private float AfternoonOffset = 180;
    private float NightOffset = 260;

    // variables between 2/3 and 1, which dictates how quickly shadows change close to noon, 
    public float ShadowLengthLerp = 0.85f;
    public float ShadowAngleLerp = 0.75f;

    public float PhaseTime = 10; // only for testing purposes, is automatically set in Game
    public bool TestingMode = false;

    private float time = 0;
    private float AngleTimer = 0;
    private int LightPhase = 1;
    private int AnglePhase = 1;
    private Quaternion StartRotation;

    public Light LightSource;
    public SpawnManager SpawnManager;

    void Start()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(MorningAngle, MorningOffest, 0);
        this.StartRotation = this.gameObject.transform.rotation;

        SpawnManager = GameObject.FindObjectOfType<SpawnManager>();
        if (!TestingMode) PhaseTime = SpawnManager.phaseDuration;

        NightAngle = MorningAngle; // still does nothing
        NightOffset = AfternoonOffset - MorningOffest + AfternoonOffset;
    }

    // Update is called once per frame
    void Update()
    {
        // checking to see if spawnManager is in a phase or not, the Break state 
        // means it is in between phases
        if (SpawnManager.state != SpawnManager.State.Break || TestingMode)
        {
            switch (LightPhase)
            {
                case 1:
                    ColorFade(DawnLight, MorningLight, PhaseTime);
                    break;
                case 2:
                    ThreeColorFade(MorningLight, AfternoonLight, DuskLight, PhaseTime);
                    break;
                case 3:
                    ColorFade(DuskLight, NightLight, PhaseTime);
                    break;
            }

            switch (AnglePhase)
            {
                case 1:
                    AngleFade(0, (AfternoonAngle - MorningAngle) * ShadowLengthLerp,
                              0, (AfternoonOffset - MorningOffest) * ShadowAngleLerp,
                              PhaseTime);
                    break;

                case 2:
                    ThreeAngleFade((AfternoonAngle - MorningAngle) * ShadowLengthLerp, AfternoonAngle, (AfternoonAngle - MorningAngle) * ShadowLengthLerp,
                                   (AfternoonOffset - MorningOffest) * ShadowAngleLerp, AfternoonOffset - MorningOffest, AfternoonOffset - MorningOffest + (AfternoonAngle - MorningAngle) * ShadowAngleLerp / 2,
                                   PhaseTime);
                    break;
                case 3:
                    AngleFade((AfternoonAngle - MorningAngle) * ShadowLengthLerp, 0,
                              AfternoonOffset - MorningOffest + (AfternoonAngle - MorningAngle) * ShadowAngleLerp/2, NightOffset - MorningOffest,
                              PhaseTime);
                    break;
            }
        }
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

    // ThreeXFade funtions do the same as the former, but with an extra midpoint
    public void ThreeColorFade(Color startC, Color midC, Color endC, float t)
    {
        if (time < 0.5)
        {
            time += Time.deltaTime / t;
            LightSource.color = Color.Lerp(startC, midC, time*2);
            // Debug.Log("Changing Light: " + time);
        }
        else if (AngleTimer < 1)
        {
            time += Time.deltaTime / t;
            LightSource.color = Color.Lerp(midC, endC, (time - 0.5f)*2);
        }
        else
        {
            LightPhase++;
            time = 0;
        }
    }

    public void ThreeAngleFade(float startX, float midX, float endX, float startY, float midY, float endY, float t)
    {
        if (AngleTimer < 0.5)
        {
            AngleTimer += Time.deltaTime / t;
            this.gameObject.transform.rotation = Quaternion.Euler(MorningAngle + Mathf.Lerp(startX, midX, AngleTimer*2),
                                                                  MorningOffest + Mathf.Lerp(startY, midY, AngleTimer*2),
                                                                  0);

            // Debug.Log("Changing X Angle: " + AngleTimer + " by " + Mathf.Lerp(startX, midX, AngleTimer*2) + " degrees; " + startX + ", " + midX);
            // Debug.Log("Changing Y Angle: " + AngleTimer + " by " + Mathf.Lerp(startY, midY, AngleTimer*2 + " degrees; " + startY + ", " + midY);
        }
        else if (AngleTimer < 1)
        {
            AngleTimer += Time.deltaTime / t;
            this.gameObject.transform.rotation = Quaternion.Euler(MorningAngle + Mathf.Lerp(midX, endX, (AngleTimer - 0.5f) * 2),
                                                                  MorningOffest + Mathf.Lerp(midY, endY, (AngleTimer - 0.5f) * 2),            
                                                                  0);
            // Debug.Log("Changing X Angle: " + AngleTimer + " by " + Mathf.Lerp(midX, endX, (AngleTimer - 0.5f) * 2) + " degrees; " + midX + ", " + endX);
            // Debug.Log("Changing Y Angle: " + AngleTimer + " by " + Mathf.Lerp(midX, endX, (AngleTimer - 0.5f) * 2) + " degrees; " + mid + ", " + endY);
        }
        else 
        {
            AnglePhase++;
            AngleTimer = 0;
        }
    }
}

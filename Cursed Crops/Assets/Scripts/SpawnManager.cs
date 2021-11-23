using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Public Variables
    public float harvestTimer;
    public string phase = "Pre"; // Stars in pre then goes to morning, afternoon and night
    public string previousphase = "Pre";
    public float morningStartTime = 5f;
    public float afternoonStartTime = 35f;
    public float nightStartTime = 65f;
    public float endTime = 95f;

    // For rn spawner works with an array. But change to a list when planting phase is in
    // Because it will be easier to dynamically add and remove to
    public static List<GameObject> spawners = new List<GameObject>();
    public GameObject[] testing;

    void Update()
    {
        harvestTimer += Time.deltaTime;

        // Change Phases
        if (harvestTimer < morningStartTime)
            phase = "Pre";
        else if(harvestTimer > morningStartTime && harvestTimer < afternoonStartTime)
            phase = "Morning";
        else if (harvestTimer > afternoonStartTime && harvestTimer < nightStartTime)
            phase = "Afternoon";
        else if (harvestTimer > nightStartTime && harvestTimer < endTime)
            phase = "Night";
        else
            phase = "Over";
    }

    private void FixedUpdate()
    {
        // If morning phase
        if (phase == "Morning")
            Spawn("Morning", "Pre");
        else if (phase == "Afternoon")
            Spawn("Afternoon", "Morning");
        else if (phase == "Night")
            Spawn("Night", "Afternoon");
    }

    private void Spawn(string currnetPhase, string prevPase)
    {
        // Loop through spawners, activate ones with morning germination
        for (int i = 0; i < testing.Length; i++)
        {
            if (testing[i].GetComponent<Spawner>().germinationPeriod == phase)
            {
                // If previous phase is the phase before phase. AKA Morning just started
                // Burst spawn at all Morning crops
                if (previousphase == prevPase)
                {
                    testing[i].GetComponent<Spawner>().Spawn("Full");
                    Debug.Log("Burst Spawn");
                }
                // It is currently Morning phase
                // Slowly spawn more enemies from crops
                else if (previousphase == phase)
                {
                    float randNum = Random.Range(0, 1000);
                    if (testing[i].GetComponent<Spawner>().potency < randNum)
                    {
                        Debug.Log("random Spawn" + randNum);
                        testing[i].GetComponent<Spawner>().Spawn("Half");
                    }
                }
                else
                {
                    Debug.LogError("previousphase did something wack: " + previousphase);
                }
            }
        }

        // Update previous phase
        if (previousphase == prevPase)
            previousphase = phase;
    }
}

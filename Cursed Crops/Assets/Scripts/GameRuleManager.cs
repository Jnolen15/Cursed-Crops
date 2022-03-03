using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    // ================ Public ================
    public int startingMoney = 100;
    public int totalPointBounty = 100;

    public int numCrop0Planted = 0;
    public int numCrop1Planted = 0;
    public int numCrop2Planted = 0;
    public int numCrop3Planted = 0;

    // ================ Private ================
    private int globalMoney = 0;
    [SerializeField] private int globalPoints = 0;

    public float getMoney() { return globalMoney; }
    public void addMoney(int amount) { globalMoney += amount; }
    public float getPoints() { return globalPoints; }
    public void addPoints(int amount) { globalPoints += amount; }

    // Start is called before the first frame update
    void Start()
    {
        globalMoney += startingMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addBountyPoints(CropSO activeCrop, int cropNum)
    {
        int timesPlanted = 0;
        switch (cropNum)
        {
            case 0:
                timesPlanted = numCrop0Planted;
                break;
            case 1:
                timesPlanted = numCrop1Planted;
                break;
            case 2:
                timesPlanted = numCrop2Planted;
                break;
            case 3:
                timesPlanted = numCrop3Planted;
                break;
        }

        // If falloff is reached
        if (timesPlanted > activeCrop.numBeforeFallOff)
        {
            Debug.Log("Lesser bounty");
            int falloff = activeCrop.bountyFallOffAmmount * (timesPlanted - activeCrop.numBeforeFallOff);
            if (falloff >= activeCrop.bountyWorth) falloff = activeCrop.bountyWorth - 2;
            addPoints(activeCrop.bountyWorth - falloff);
        }
        // If fallof isn't reached
        else
        {
            Debug.Log("Full bounty");
            addPoints(activeCrop.bountyWorth);
        }
    }

    // Returns true if current bounty score is greater than or equal to given percentage of total bounty
    // Float given should be (0, 1]
    public bool bountyMet(float percentage)
    {
        int temp = (int)(totalPointBounty * percentage);
        if (globalPoints >= temp) return true;
        else
        {
            Debug.Log(globalPoints + " < " + temp);
            return false;
        }
    }
}

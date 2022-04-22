using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    // ================ Public ================
    public int startingMoney = 100;
    public int totalPointBounty = 100;

    public int numCornonPlanted = 0;
    public int numGrabbagePlanted = 0;
    public int numMediBerryPlanted = 0;
    public int numScarrotPlanted = 0;
    public int numSabatamatoPlanted = 0;

    /* Difficulty increases by 1 for each successive phase.
     * It also increases by 1 for each player beyond the first.
     * What difficulty effects:
     * - Adds to the number of basic enemies spawned each burst
    */
    public int difficulty = 0;

    // ================ Private ================
    [SerializeField] private int globalPoints = 0;
    private int globalMoney = 0;
    private int players = 0;
    private PlayerManager pm;
    private GameObject textPopUp;

    public float getMoney() { return globalMoney; }
    public void addMoney(int amount) { globalMoney += amount; }
    public float getPoints() { return globalPoints; }
    public void addPoints(int amount) { globalPoints += amount; }

    // Start is called before the first frame update
    void Start()
    {
        globalMoney += startingMoney;

        pm = GameObject.Find("Player Manager").GetComponent<PlayerManager>();

        textPopUp = Resources.Load<GameObject>("Effects/TextPopUp");
    }

    // Update is called once per frame
    void Update()
    {
        // Increase difficulty for each player beyond the first
        if(players < pm.players.Count)
        {
            if (players > 1)
                difficulty++;
            players++;
        }
    }

    public void addBountyPoints(CropSO activeCrop, GameObject crop)
    {
        int timesPlanted = 0;
        switch (crop.GetComponent<Spawner>().plantName)
        {
            case "Cornon":
                timesPlanted = numCornonPlanted++;
                Debug.Log("Cornon bounty added");
                break;
            case "Grabbage":
                timesPlanted = numGrabbagePlanted++;
                Debug.Log("Grabbage bounty added");
                break;
            case "MediBerry":
                timesPlanted = numMediBerryPlanted++;
                Debug.Log("MediBerry bounty added");
                break;
            case "Scarrot":
                timesPlanted = numScarrotPlanted++;
                Debug.Log("Scarrot bounty added");
                break;
            case "Sabatamato":
                timesPlanted = numSabatamatoPlanted++;
                Debug.Log("Sabatamato bounty added");
                break;
        }

        // If falloff is reached
        if (timesPlanted > activeCrop.numBeforeFallOff)
        {
            Debug.Log("Lesser bounty");
            int falloff = activeCrop.bountyFallOffAmmount * (timesPlanted - activeCrop.numBeforeFallOff);
            if (falloff >= activeCrop.bountyWorth) falloff = activeCrop.bountyWorth - 2;
            addPoints(activeCrop.bountyWorth - falloff);
            crop.GetComponent<Spawner>().bountyWorth = activeCrop.bountyWorth - falloff;
            SpawnText(crop.transform.position, Color.green, activeCrop.bountyWorth - falloff);
        }
        // If fallof isn't reached
        else
        {
            //Debug.Log("Full bounty");
            addPoints(activeCrop.bountyWorth);
            crop.GetComponent<Spawner>().bountyWorth = activeCrop.bountyWorth;
            SpawnText(crop.transform.position, Color.green, activeCrop.bountyWorth);
        }
    }

    public void subtractBountyPoints(GameObject crop)
    {
        addPoints(-crop.GetComponent<Spawner>().bountyWorth);
        SpawnText(crop.transform.position, Color.red, -crop.GetComponent<Spawner>().bountyWorth);

        switch (crop.GetComponent<Spawner>().plantName)
        {
            case "Cornon":
                numCornonPlanted--;
                Debug.Log("Cornon bounty added");
                break;
            case "Grabbage":
                numGrabbagePlanted--;
                Debug.Log("Grabbage bounty added");
                break;
            case "MediBerry":
                numMediBerryPlanted--;
                Debug.Log("MediBerry bounty added");
                break;
            case "Scarrot":
                numScarrotPlanted--;
                Debug.Log("Scarrot bounty added");
                break;
            case "Sabatamato":
                numSabatamatoPlanted--;
                Debug.Log("Sabatamato bounty added");
                break;
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
            //Debug.Log(globalPoints + " < " + temp);
            return false;
        }
    }

    public void SpawnText(Vector3 pos, Color color, int num)
    {
        var tPopUp = Instantiate(textPopUp, pos, textPopUp.transform.rotation).GetComponent<TextPopup>();
        tPopUp.Setup(num, color);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    // ================ Public ================
    public popUpUISO popupUI;

    public int startingMoney = 100;
    public int totalPointBounty = 100;

    public int numCornonPlanted = 0;
    public int numGrabbagePlanted = 0;
    public int numMediBerryPlanted = 0;
    public int numScarrotPlanted = 0;
    public int numSabomatoPlanted = 0;

    /* Difficulty increases by 1 for each successive phase.
     * It also increases by 1 for each player beyond the first.
     * What difficulty effects:
     * - Adds to the number of basic enemies spawned each burst
    */
    public int difficulty = 0;

    // ================ Private ================
    [SerializeField] private int globalPoints = 0;
    private int globalMoney = 0;
    public int players = 0;
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
        /*if(players < pm.players.Count)
        {
            players++;
            if (players > 1) {
                difficulty++;
                Debug.Log("+1 difficulty for extra player");
            }
        }*/
    }

    public void incrementDifficulty()
    {
        difficulty += pm.players.Count;
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
            case "Sabomato":
                timesPlanted = numSabomatoPlanted++;
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
            string strNum = "+" + (activeCrop.bountyWorth - falloff).ToString();
            SpawnText(crop.transform.position, Color.green, strNum);
        }
        // If fallof isn't reached
        else
        {
            //Debug.Log("Full bounty");
            addPoints(activeCrop.bountyWorth);
            crop.GetComponent<Spawner>().bountyWorth = activeCrop.bountyWorth;
            string strNum = "+" + activeCrop.bountyWorth.ToString();
            SpawnText(crop.transform.position, Color.green, strNum);
        }
    }

    public void subtractBountyPoints(GameObject crop)
    {
        addPoints(-crop.GetComponent<Spawner>().bountyWorth);
        string strNum = "-" + crop.GetComponent<Spawner>().bountyWorth.ToString();
        SpawnText(crop.transform.position, Color.red, strNum);

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
            case "Sabomato":
                numSabomatoPlanted--;
                Debug.Log("Sabatamato bounty added");
                break;
        }
    }

    public int GetBountyWorth(CropSO activeCrop)
    {
        int timesPlanted = 0;
        switch (activeCrop.cropName)
        {
            case "Cornon":
                timesPlanted = numCornonPlanted;
                break;
            case "Grabbage":
                timesPlanted = numGrabbagePlanted;
                break;
            case "MediBerry":
                timesPlanted = numMediBerryPlanted;
                break;
            case "Scarrot":
                timesPlanted = numScarrotPlanted;
                break;
            case "Sabomato":
                timesPlanted = numSabomatoPlanted;
                break;
        }

        // If falloff is reached
        if (timesPlanted > activeCrop.numBeforeFallOff)
        {
            int falloff = activeCrop.bountyFallOffAmmount * (timesPlanted - activeCrop.numBeforeFallOff);
            if (falloff >= activeCrop.bountyWorth) falloff = activeCrop.bountyWorth - 2;
            return (activeCrop.bountyWorth - falloff);
        }
        // If fallof isn't reached
        else
        {
            return(activeCrop.bountyWorth);
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

    public void SpawnText(Vector3 pos, Color color, string text)
    {
        var tPopUp = Instantiate(textPopUp, pos, textPopUp.transform.rotation).GetComponent<TextPopup>();
        tPopUp.Setup(text, color);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    private GameObject house;
    private EnemyDamageObjective EDO;
    private GameObject p1;
    private PlayerControler PC;
    private EnemyPlayerDamage EPD;
    public GameObject p1UI;
    // UI elements must me dragged/droppped
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI vegetableText;
    public Slider HealthBar;
    public Slider HouseHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        // aquiring player controller
        // dependant on player name, might need to be improved
        // PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>();
        house = GameObject.Find("Objective");
        EDO = house.GetComponent<EnemyDamageObjective>();
    } 

    // Update is called once per frame
    void Update()
    {
        if (PC == null && GameObject.Find("Player(Clone)") != null)
        {
            p1 = GameObject.Find("Player(Clone)");
            PC = p1.GetComponent<PlayerControler>();
            EPD = p1.GetComponent<EnemyPlayerDamage>();

            // also need to catch the error created by not finding a player initially
            p1UI.SetActive(true);
            // players prefabs currenty don't store their own name as far as I can tell
            nameText.text = "Harvey";
        }

        // checking if PC has been aquired
        if (PC != null)
        {
            // managing  currency text
            float currMoney = PC.gameObject.GetComponent<PlayerResourceManager>().getMoney();
            float currRedCrops = PC.gameObject.GetComponent<PlayerResourceManager>().getRedCrops();
            float currPurpleCrops = PC.gameObject.GetComponent<PlayerResourceManager>().getPurpleCrops();

            currencyText.text = "Money: " + currMoney;
            vegetableText.text = "R: " + currRedCrops + ", P: " + currPurpleCrops;
            // managing health bar
            HealthBar.value = (float)EPD.playerHealth / (float)EPD.reviveHealth;
        }

        // updating general UI
        HouseHealthBar.value = (float)EDO.houseHealth / (float)EDO.startingHouseHealth;
    }

    
}

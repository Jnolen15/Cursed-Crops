using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    
    private PlayerManager PM;
    private PlayerControler PC1;
    private PlayerControler PC2;
    private EnemyPlayerDamage EPD1;
    private EnemyPlayerDamage EPD2;
    private PlayerResourceManager PR1;
    private PlayerResourceManager PR2;


    // UI elements must be dragged/droppped
    // p1 UI elements
    public GameObject p1UI;
    public TextMeshProUGUI nameText1;
    public TextMeshProUGUI currencyText1;
    public TextMeshProUGUI vegetableText1;
    public Slider HealthBar1;

    // p2 UI elements
    public GameObject p2UI;
    public TextMeshProUGUI nameText2;
    public TextMeshProUGUI currencyText2;
    public TextMeshProUGUI vegetableText2;
    public Slider HealthBar2;

    // general UI elements
    public Slider HouseHealthBar;
    private GameObject house;
    private EnemyDamageObjective EDO;



    // Start is called before the first frame update
    void Start()
    {
        // aquiring player manager (for player list) and house object and damage manager
        // PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>();
        house = GameObject.Find("Objective");
        EDO = house.GetComponent<EnemyDamageObjective>();
        PM = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
    } 

    // Update is called once per frame
    void Update()
    {
        // updating general UI
        HouseHealthBar.value = (float)EDO.houseHealth / (float)EDO.startingHouseHealth;

        // updating p1 UI
        if (PC1 == null && PM.players.Count >= 1)
        {
            // p1 = GameObject.Find("Player(Clone)");
            PC1 = PM.players[0].GetComponent<PlayerControler>();
            PR1 = PM.players[0].GetComponent<PlayerResourceManager>();


            // also need to catch the error created by not finding a player initially
            EPD1 = PM.players[0].GetComponent<EnemyPlayerDamage>();
            p1UI.SetActive(true);

            // setting player name
            nameText1.text = PM.players[0].GetComponent<PlayerAnimOCManager>().selectedCharacter.ToString();
        }

        // checking if PC has been aquired
        if (PC1 != null)
        {
            // managing  currency text
            currencyText1.text = "Money: " + PR1.getMoney();
            vegetableText1.text = "R: " + PR1.getRedCrops() + ", P: " + PR1.getPurpleCrops();
            // managing health bar
            HealthBar1.value = (float)EPD1.playerHealth / (float)EPD1.reviveHealth;
         }

        // updating p2 UI
        if (PC2 == null && PM.players.Count >= 2)
        {
            // p1 = GameObject.Find("Player(Clone)");
            PC2 = PM.players[1].GetComponent<PlayerControler>();
            PR2 = PM.players[1].GetComponent<PlayerResourceManager>();


            // also need to catch the error created by not finding a player initially
            EPD2 = PM.players[1].GetComponent<EnemyPlayerDamage>();
            p2UI.SetActive(true);

            // setting player name
            nameText2.text = PM.players[1].GetComponent<PlayerAnimOCManager>().selectedCharacter.ToString();
        }

        // checking if PC has been aquired
        if (PC2 != null)
        {
            // managing  currency text
            currencyText2.text = "Money: " + PR2.getMoney();
            vegetableText2.text = "R: " + PR2.getRedCrops() + ", P: " + PR2.getPurpleCrops();
            // managing health bar
            HealthBar2.value = (float)EPD2.playerHealth / (float)EPD2.reviveHealth;
        }
    }
}


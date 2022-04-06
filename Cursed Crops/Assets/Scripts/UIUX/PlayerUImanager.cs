using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUImanager : MonoBehaviour
{
    // General Variables
    public PlayerManager PM;
    public PlayerControler PC;
    public EnemyPlayerDamage EPD;
    private PlayerResourceManager PR;

    // UI Elements Variables
    public GameObject PlayerUI; // obselete, us this.GameObject
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI CropText;
    public Slider HealthBar;
    public Slider ReviveBar;
    public TextMeshProUGUI CurHealth;
    public TextMeshProUGUI MaxHealth;
    public AmmoCounter AmmoCounter;

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    // Initializes the UI, takes in the player #
    public void Initialize(int playerNum)
    {
        // enabling GameObject
        this.gameObject.SetActive(true);

        // adjusting for array index and acquiring key scripts
        playerNum -= 1;
        PM = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        PC = PM.players[playerNum].GetComponent<PlayerControler>();
        PR = PM.players[playerNum].GetComponent<PlayerResourceManager>();
        EPD = PM.players[playerNum].GetComponent<EnemyPlayerDamage>();

        // setting player name
        NameText.text = PM.players[playerNum].GetComponentInChildren<PlayerAnimOCManager>().selectedCharacter.ToString();
    }

    // updates the UI (can be moved to the update function now)
    public void UpdateUI()
    {
        // managing  currency text
        CropText.text = PR.getCrops() + " / " + PR.maxCrops;

        // managing health bar/revive 
        HealthBar.value = (float)EPD.playerHealth / (float)EPD.reviveHealth;
        ReviveBar.value = EPD.reviveTimer / EPD.reviveTime;
        CurHealth.text = EPD.playerHealth.ToString();
        MaxHealth.text = EPD.reviveHealth.ToString();

        // managing ammo UI
        AmmoCounter.SetBullets(PC.curBullets, PC.GetRangeCD());
    }

}

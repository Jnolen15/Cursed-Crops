using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    
    // external scripts/Objects
    private PlayerManager PM;
    private SpawnManager SM;
    private GameRuleManager GRM;

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
    public TextMeshProUGUI vegetableText1;
    public Slider HealthBar1;
    public AmmoCounter AmmoCounter1;

    // p2 UI elements
    public GameObject p2UI;
    public TextMeshProUGUI nameText2;
    public TextMeshProUGUI vegetableText2;
    public Slider HealthBar2;
    public AmmoCounter AmmoCounter2;

    // general UI elements
    public Slider HouseHealthBar;
    private GameObject house;
    private EnemyDamageObjective EDO;

    public Slider PhaseTimer;
    public TextMeshProUGUI PhaseCounter;
    public TextMeshProUGUI moneyText;

    public GameObject QuotaOverlay;
    public Slider QuotaBar;
    public TextMeshProUGUI QuotaText;


    


    public GameObject[] WaveBars = new GameObject[8];



    // Start is called before the first frame update
    void Start()
    {
        // aquiring player manager (for player list) and house object and damage manager
        // PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>();
        house = GameObject.Find("Objective");
        EDO = house.GetComponent<EnemyDamageObjective>();
        PM = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        SM = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        GRM = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();

        setUpTimer();
    } 

    // Update is called once per frame
    void Update()
    {
        // updating general UI
        HouseHealthBar.value = (float)EDO.houseHealth / (float)EDO.startingHouseHealth;
        moneyText.text = "Money: " + GRM.getMoney();
        UpdateTimer();

        // quota bar is enabled between phases
        if (SM.state == SpawnManager.State.Break)
        {
            QuotaOverlay.SetActive(true);
            QuotaText.text = "Quota: " + GRM.getPoints() + " / " + SM.getQuota();
            QuotaBar.value = GRM.getPoints() / SM.getQuota();
        } else
        {
            QuotaOverlay.SetActive(false);
        }

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
            nameText1.text = PM.players[0].GetComponentInChildren<PlayerAnimOCManager>().selectedCharacter.ToString();
        }

        // checking if PC has been aquired
        if (PC1 != null)
        {
            // managing  currency text
            vegetableText1.text = "Crops: " + PR1.getCrops() + " / " + PR1.maxCrops;
            // managing health bar
            HealthBar1.value = (float)EPD1.playerHealth / (float)EPD1.reviveHealth;
            // managing ammo UI
            AmmoCounter1.SetBullets(PC1.curBullets);
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
            nameText2.text = PM.players[1].GetComponentInChildren<PlayerAnimOCManager>().selectedCharacter.ToString();
        }

        // checking if PC has been aquired
        if (PC2 != null)
        {
            // managing  currency text
            vegetableText2.text = "Crops: " + PR2.getCrops() + " / " + PR2.maxCrops;
            // managing health bar
            HealthBar2.value = (float)EPD2.playerHealth / (float)EPD2.reviveHealth;
            // managing ammo UI
            AmmoCounter2.SetBullets(PC2.curBullets);
        }
    }

    private void UpdateTimer()
    {
        // phase counter
        PhaseCounter.text = SM.currentPhase;

        if (SM.state != SpawnManager.State.Break)
        {
            PhaseTimer.value = (SM.elapsedTime - (SM.currentPhaseEndTime - SM.phaseDuration)) / SM.phaseDuration;
        } else
        {
            PhaseTimer.value = 0;
        }
    }

    // places Wavebars dynamically according to # of waves
    // warning: bars have to already exist in the slider and be put into the WaveBars Array, otherwise this will break (currently 8)
    private void setUpTimer()
    {
        for (int i = 0; i < SM.wavesPerPhase; i++)
        {
            // the -10 is adjusting for the space inbetween the slider's rect and the fill's rect
            float width = PhaseTimer.GetComponent<RectTransform>().rect.width - 10;
            Transform trans = WaveBars[i].GetComponent<Transform>();
            Vector3 newTrans = trans.localPosition;
            newTrans.x = -width / 2 + (width / SM.wavesPerPhase * i);
            trans.localPosition = newTrans;
        }

        // hides surplus wave bars
        for (int i = SM.wavesPerPhase; i < WaveBars.Length; i++)
        {
            WaveBars[i].SetActive(false);
        }
    }
}


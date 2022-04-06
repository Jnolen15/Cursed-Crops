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


    // player UIs, must be dragged/droppped
    public PlayerUImanager p1UImanager;
    public PlayerUImanager p2UImanager;

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
            QuotaText.text = "Plant Crops to Fill Quota: " + GRM.getPoints() + " / " + SM.getQuota();
            QuotaBar.value = GRM.getPoints() / SM.getQuota();
        } else
        {
            QuotaOverlay.SetActive(false);
        }


        // initializing Player 1 UI if necessary, automatically updates itself
        if (PM.players.Count >= 1 && !p1UImanager.gameObject.activeSelf)
        {
            p1UImanager.Initialize(1);
        }

        // initializing Player 2 UI if necessary, automatically updates itself
        if (PM.players.Count >= 2 && !p2UImanager.gameObject.activeSelf)
        {
            p2UImanager.Initialize(2);
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


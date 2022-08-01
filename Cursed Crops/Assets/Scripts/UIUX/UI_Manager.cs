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
    public PlayerUImanager p3UImanager;
    public PlayerUImanager p4UImanager;

    // general UI elements
    public Slider HouseHealthBar;
    private GameObject house;
    private EnemyDamageObjective EDO;

    public Sprite morningIcon;
    public Sprite afternoonIcon;
    public Sprite nightIcon;

    public Image PhaseIcon;
    public Slider PhaseTimer;
    public TextMeshProUGUI PhaseCounter;
    public TextMeshProUGUI moneyText;

    public GameObject QuotaOverlay;
    public Slider QuotaBar;
    public TextMeshProUGUI QuotaText;

    public GameObject[] WaveBars = new GameObject[8];

    private float money;
    private float quota;

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

        money = GRM.getMoney();

        setUpTimer();
    } 

    // Update is called once per frame
    void Update()
    {
        // updating general UI
        HouseHealthBar.value = (float)EDO.houseHealth / (float)EDO.startingHouseHealth;

        if (money != GRM.getMoney())
        {
            StopAllCoroutines();
            if (money > GRM.getMoney())
                StartCoroutine(TextColorFlash(moneyText, Color.red, 0.5f));
            else
                StartCoroutine(TextColorFlash(moneyText, Color.green, 0.5f));
            money = GRM.getMoney();
        }

        moneyText.text = "Money: " + GRM.getMoney();
        UpdateTimer();

        // quota bar is enabled between phases
        if (SM.state == SpawnManager.State.Break)
        {
            QuotaOverlay.SetActive(true);
            QuotaText.text = "Plant Crops to Fill Quota: " + GRM.getPoints() + " / " + SM.getQuota();
            QuotaBar.value = GRM.getPoints() / SM.getQuota();

            if (quota != GRM.getPoints())
            {
                StopAllCoroutines();
                if (quota > GRM.getPoints())
                    StartCoroutine(TextColorFlash(QuotaText, Color.red, 0.5f));
                else
                    StartCoroutine(TextColorFlash(QuotaText, Color.green, 0.5f));
                quota = GRM.getPoints();
            }

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

        if (PM.players.Count >= 3 && !p3UImanager.gameObject.activeSelf)
        {
            p3UImanager.Initialize(3);
        }

        if (PM.players.Count >= 4 && !p4UImanager.gameObject.activeSelf)
        {
            p4UImanager.Initialize(4);
        }
    }

    private void UpdateTimer()
    {
        // phase counter
        PhaseCounter.text = SM.currentPhase;
        if (PhaseCounter.text == "Morning") PhaseIcon.sprite = morningIcon;
        else if (PhaseCounter.text == "Afternoon") PhaseIcon.sprite = afternoonIcon;
        else if (PhaseCounter.text == "Night") PhaseIcon.sprite = nightIcon;

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

    IEnumerator TextColorFlash(TextMeshProUGUI text, Color color, float time)
    {
        text.color = color;
        yield return new WaitForSeconds(time);
        text.color = Color.black;
    }
}


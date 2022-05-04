using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestFlag : MonoBehaviour
{
    // ================ Public ================
    public int totalPlayers;
    public int playersReady;

    // ================ Private ================
    private SpawnManager spawnManager;
    public PlayerManager playerManager;
    private GameRuleManager grm;
    private GameObject quotaWarning;        // Placeholder text to tell players quota isn't met
    private List<GameObject> playersIn = new List<GameObject>();
    private float startPause = 3f;
    private GameObject flag;
    private bool countdownStarted = false;
    // Note: These have to be manualy set in the prefab with its tilt matching the leaned sprites in order to look good
    private Vector3 flagStartPos = new Vector3(-0.4f, 0.4f, 0.1f);
    private Vector3 flagEndPos = new Vector3(-0.03f, 1.4f, 1.1f);

    void Start()
    {
        // Game Rule Manager
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();

        // Quota popup notification
        quotaWarning = this.transform.GetChild(0).gameObject;
        quotaWarning.SetActive(false);

        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        spawnManager = this.transform.parent.gameObject.GetComponent<SpawnManager>();

        flag = transform.Find("FlagFlag").gameObject;
    }

    void Update()
    {
        // Set players
        if(playerManager.players.Count < 1)
            totalPlayers = 1;
        else
            totalPlayers = playerManager.players.Count;

        // Start wave if players are ready
        if(totalPlayers == playersReady)
        {
            // Before each wave, make sure bounty is met before starting
            if (spawnManager.currentPhase == "Morning") checkStartPhase(0.2f);
            else if (spawnManager.currentPhase == "Afternoon") checkStartPhase(0.5f);
            else if (spawnManager.currentPhase == "Night") checkStartPhase(1);
        } else
        {
            flag.transform.localPosition = flagStartPos;
            if (countdownStarted)
                StopAllCoroutines();
            countdownStarted = false;
        }

        if (playersReady < 0) playersReady = 0;
    }

    private void checkStartPhase(float percent)
    {
        if (grm.bountyMet(percent))
        {
            if (!countdownStarted)
            {
                StopAllCoroutines();
                StartCoroutine(BeginHarvest());
            }
        }
        else
        {
            quotaWarning.SetActive(true);
        }
    }

    IEnumerator BeginHarvest()
    {
        countdownStarted = true;
        float time = 0;
        Vector3 startpos = flag.transform.localPosition;
        while (time < startPause)
        {
            flag.transform.localPosition = Vector3.Lerp(startpos, flagEndPos, time / startPause);
            time += Time.deltaTime;
            yield return null;
        }
        flag.transform.localPosition = flagEndPos;
        spawnManager.StartHarvest();
        quotaWarning.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!playersIn.Contains(other.gameObject))
            {
                playersIn.Add(other.gameObject);
                playersReady++;
                Debug.Log(other.gameObject.name + " has entered the flag.");
            }
        }
        
        // OLD Melee attack to ready way
        /*if (other.gameObject.name == "MeleeAttack")
        {
            Debug.Log("PLAYER HIT HARVEST FLAG");
            bool playerReady = other.gameObject.GetComponentInParent<PlayerControler>().ready;
            if (playerReady)
            {
                playerReady = false;
                playersReady--;
            } else
            {
                playerReady = true;
                playersReady++;
            }
            other.gameObject.GetComponentInParent<PlayerControler>().ready = playerReady;
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playersIn.Contains(other.gameObject))
            {
                playersIn.Remove(other.gameObject);
                playersReady--;
                Debug.Log(other.gameObject.name + " has left the flag.");
            }
        }
    }

    private void OnEnable()
    {
        playersReady = 0;
        countdownStarted = false;
    }

    private void OnDisable()
    {
        playersReady = 0;
        countdownStarted = false;
    }
}

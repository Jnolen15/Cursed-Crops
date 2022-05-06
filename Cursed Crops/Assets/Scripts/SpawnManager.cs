using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    // ================ Public ================
    [Header("Each phase has X waves and X-1 puases. (There is no pause after the last wave)")]
    public float phaseDuration;             // Duration of each phase in seconds
    public float waveDuration = 30f;        // Duration of each wave in seconds
    public float pauseDuration = 10f;       // Duration of each short pause between waves in seconds
    public int wavesPerPhase = 4;           // The # of waves in each phase. Make sure the time values add up correctly
    public int spawnersPerPhase = 4;        // Number of spawners created before each phase

    public float elapsedTime;               // Time since level started
    public string currentPhase = "Pre";     // Stars in Pre then goes to Morning, Afternoon and Night
    public int currentWave = 0;             // Increments up for each wave in the phase

    public GameObject harvestStartFlag;     // Some object that, when each player interacts with it, begins the harvest.
    private GameObject harvestFlag;
    public GameObject placementChecker;     // A prefab used to check if a location is suitable for a crop spawn
    public GameObject meleeEnemy;           // The basic melee enemy to spawn
    public GameObject rangeEnemy;           // The basic range enemy to spawn
    public GameObject spawnAnimator;        // Used to make the enemy look like they are rising from the ground

    public float xbounds = 18f;
    public float ybounds = 18f;

    public float gridSize = 1;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    public AudioClip waveStartSound;
    public AudioClip harvestMusic;
    public AudioClip plantingMusic;
    public AudioSource daMusic;             //Will get the music object from the scene (Music makes you lose control)

    // ================ Private ================
    public float currentPhaseEndTime = 0f;   // The time the current phase should end
    private float currentWaveEndTime = 0f;   // The time the current wave should end
    private float currentPauseEndTime = 0f;  // The time the current pause should end
    private float basicSpawnInterval = 0f;     // Used to spawn basic enemies at certain intervals
    private float specialSpawnInterval = 0f; // USed to spawn special enemies at specific intervals
    private float specialSpawnTime = 0f;
    private int specialToSpawn = 0;
    private bool timerStarted = false;
    private bool gridUpdated = false;
    private delegate void Callback();
    private GameObject gridChild;
    private GameRuleManager grm;
    public List<GameObject> spawners = new List<GameObject>();  // List of active enemy spawners
    private Dictionary<Vector3, string> spawnerGridPositions = new Dictionary<Vector3, string>(); // Dictionary of grid positions on plantable tiles
    private Vector3[] positions;    // array version of Dictionary keys, used to get a random position in the dictionary
    public enum State
    {
        Wave,
        Pause,
        Break
    }
    public State state;

    private void Start()
    {
        // Get reffrences
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
        daMusic = GameObject.Find("Music").GetComponent<AudioSource>();

        // calculate phase duration
        phaseDuration = waveDuration * wavesPerPhase + pauseDuration * wavesPerPhase;

        // Spawn harvest start flag
        harvestFlag = this.transform.GetChild(0).gameObject;
        harvestFlag.SetActive(false);

        gridChild = this.transform.GetChild(1).gameObject;

        CreateSpawnerGrid();
    }

    void Update()
    {
        if(state != State.Break)
            elapsedTime += Time.deltaTime;

        if (!gridUpdated && gridChild.transform.GetChild(0).GetComponent<GridPlacementChecker>().colChecked)
        {
            UpdateSpawnerPositions();
            gridUpdated = true;
        }

        //Debug.Log("There are " + gridChild.transform.childCount + " grid children");

        if (gridUpdated)
            RunSpawnSystem();
    }

    private void RunSpawnSystem()
    {
        // Phase timer. If the current phase has ended
        if (currentPhaseEndTime < elapsedTime)
        {
            // Set state to Break to prevent more enemy spawning
            state = State.Break;

            // Phase is over but wait for all enemies to die
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                currentPhaseEndTime = elapsedTime + phaseDuration;
                daMusic.clip = plantingMusic;
                daMusic.Play();
                if (currentPhase == "Pre")
                {
                    grm.incrementDifficulty();
                    currentPhase = "Morning";
                    harvestFlag.SetActive(true);
                }
                else if (currentPhase == "Morning")
                {
                    StopAllCoroutines();
                    grm.incrementDifficulty();
                    currentPhase = "Afternoon";
                    harvestFlag.SetActive(true);
                    DestroySpawners();
                }
                else if (currentPhase == "Afternoon")
                {
                    StopAllCoroutines();
                    grm.incrementDifficulty();
                    currentPhase = "Night";
                    harvestFlag.SetActive(true);
                    DestroySpawners();
                }
                else if (currentPhase == "Night")
                {
                    StopAllCoroutines();
                    currentPhase = "Post";
                    DestroySpawners();
                }
            }
            else
            {
                Debug.Log("Found " + GameObject.FindGameObjectsWithTag("Enemy").Length + " enemies");
            }
        }

        // Wave and Pause timer
        if (!timerStarted && state != State.Break)
        {
            if (state == State.Wave)
            {
                timerStarted = true;
                StartCoroutine(timer(() => { state = State.Pause; timerStarted = false; }, waveDuration));
                if (currentWaveEndTime < elapsedTime)
                {
                    currentWave++;

                    currentWaveEndTime = elapsedTime + waveDuration;
                }
            }
            else if (state == State.Pause)
            {
                timerStarted = true;
                StartCoroutine(timer(() => { state = State.Wave; timerStarted = false; }, pauseDuration));
                if (currentPauseEndTime < elapsedTime)
                {
                    currentPauseEndTime = elapsedTime + pauseDuration;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == State.Wave && currentPhase != "Pre")
        {
            BasicSpawn();
            WaveSpawn("Full");
        }
            
    }

    public void StartHarvest()
    {
        state = State.Wave;
        // Play sound, start music
        gameObject.GetComponent<AudioPlayer>().PlaySound(waveStartSound);
        daMusic.clip = harvestMusic;
        daMusic.Play();
        // Deactivate harvest flag
        harvestFlag.SetActive(false);
        // Calculate the array holding the grid positions.
        positions = spawnerGridPositions.Keys.ToArray();
        // Calculate special enemy spawn interval
        specialSpawnInterval = (phaseDuration - pauseDuration) / spawners.Count;
        specialSpawnTime = specialSpawnInterval + elapsedTime;
        specialToSpawn = 0;
        Debug.Log("Will spawn special every " + specialSpawnInterval + " seconds.");
    }


    // ============ SPAWNER GRID SETUP STUFF ============
    private void CreateSpawnerGrid()
    {
        for(float i = -xbounds/2; i <= (xbounds/2) - 1; i++)
        {
            for (float j = -ybounds/2; j <= (ybounds/2) - 1; j++)
            {
                Vector3 testPos = new Vector3(i, 1f, j);
                GameObject newChecker = Instantiate(placementChecker, testPos, transform.rotation, gridChild.transform);
                alignToGrid(newChecker.transform);
                //Debug.Log("Created a spawner at: " + newChecker.transform.position);
            }
        }
    }

    private void UpdateSpawnerPositions()
    {
        //Debug.Log("There are " + gridChild.transform.childCount + " Grid children.");
        for (int i = 0; i < gridChild.transform.childCount; i++)
        {
            gridChild.transform.GetChild(i).GetComponent<GridPlacementChecker>().AddSelf(spawnerGridPositions);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(xbounds, 1, ybounds));
    }

    // ============ ENEMY SPAWNING ============
    // Spawn basic enemies at random positions in available locations.
    private void BasicSpawn()
    {
        // At random intervals, spawn a random ammount
        if (basicSpawnInterval <= elapsedTime)
        {
            // Update timer and spawn values
            basicSpawnInterval = elapsedTime + 6f;
            float numToSpawn = Random.Range(grm.difficulty, (4 + grm.difficulty));

            // Spawn the select number of enemies
            for (int i = 0; i < numToSpawn; i++)
            {
                // Pick a location
                Vector3 pos = new Vector3(0, 0, 0);
                if (positions.Length > 0)
                    pos = positions[Random.Range(0, positions.Length - 1)];
                else
                    Debug.LogError("Dictionary has a length of: " + positions.Length);

                string availablePos;
                if (spawnerGridPositions.TryGetValue(pos, out availablePos))
                {
                    // Success
                    if (availablePos == "Empty")
                    {
                        // Decide which enemy to spawn and spawn it
                        GameObject selectedEnemy = null;
                        float rand = Random.Range(1, 10);
                        if (rand < 7) selectedEnemy = meleeEnemy;
                        else selectedEnemy = rangeEnemy;
                        GameObject sa = Instantiate(spawnAnimator, pos, transform.rotation);
                        sa.GetComponent<SpawnAnimator>().AnimateSpawn(selectedEnemy, pos);
                        //Instantiate(selectedEnemy, pos, transform.rotation);
                        //Instantiate(Resources.Load<GameObject>("Effects/EnemySpawnParticle"), pos, transform.rotation).GetComponent<ParticleSystem>();
                    }
                    else
                    {
                        Debug.Log("Position Taken");
                        i--;
                    }
                }
                else
                    Debug.LogError("Dictionary error");
            }
        }
    }

    //Spawn enemies at plants as the wave progresses. Time between spawns is measured by a spawners potency
    private void WaveSpawn(string type)
    {
        // New system. Based on how many spawners there are, spawn them evenly throught the wave
        if (elapsedTime >= specialSpawnTime)
        {
            Spawner currentSpawner = spawners[specialToSpawn].GetComponent<Spawner>();
            currentSpawner.Spawn(type);
            currentSpawner.lastTimeSpawned = elapsedTime;

            // Update counters
            specialToSpawn++;
            specialSpawnTime += specialSpawnInterval;
        }

        // OLD SPAWNING METHOD: For both basic and special spawners
        /*foreach (GameObject obj in spawners)
        {
            Spawner currentSpawner = obj.GetComponent<Spawner>();
            if (currentSpawner.lastTimeSpawned == 0) // Initial spawn
            {
                float rand = Random.Range(1, 10);
                // 60% chance to immediatly spawn, 40% to spawn sometime over the next 5 seconds
                if (rand < 6)
                {
                    currentSpawner.Spawn(type);
                    currentSpawner.lastTimeSpawned = elapsedTime + Random.Range(0f, 5f);
                }
                else
                {
                    // subtract by current potency here so it won't wait the bonus ammount + the potency
                    currentSpawner.lastTimeSpawned = elapsedTime + (Random.Range(0f, currentSpawner.potency) - currentSpawner.potency);
                }
            }
            else if ((currentSpawner.lastTimeSpawned + currentSpawner.potency) <= elapsedTime)
            {
                currentSpawner.Spawn(type);
                currentSpawner.lastTimeSpawned = elapsedTime;
            }
        }*/
    }

    public void AddSpawner(GameObject spawner)
    {
        spawners.Add(spawner);
    }

    public void RemoveSpawner(GameObject spawner)
    {
        if(spawners.Find(gameObject => spawner))
            spawners.Remove(spawner);
    }

    private void DestroySpawners()
    {
        foreach (GameObject obj in spawners)
        {
            Destroy(obj);
        }

        spawners.Clear();
    }


    // ============ OTHER ============
    private void alignToGrid(Transform trans)
    {
        // Get player position
        Vector3 selectedPos = trans.position;

        // align it to grid
        float xPos = Mathf.Round(selectedPos.x);
        xPos -= (xPos % gridSize);
        float zPos = Mathf.Round(selectedPos.z);
        zPos -= (zPos % gridSize);

        // Add offset
        xPos += gridOffsetX;
        zPos += gridOffsetZ;

        // Create and return position
        Vector3 placePos = new Vector3(xPos, selectedPos.y, zPos);

        trans.position = placePos;
    }

    IEnumerator timer(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    private void OnDisable()
    {
        //Debug.Log("Called disable from spawnmanager");
        //spawnerGridPositions.Clear();
    }

    // temp solution for getting quota #s, will need to be changed
    public int getQuota()
    {
        if (currentPhase == "Morning")
        {
            return 20;
        } else if (currentPhase == "Afternoon")
        {
            return 50;
        } else if (currentPhase == "Night") {
            return 100;
        }
        return -1;
    }


    // ============================ OLD SPAWNING SYSTEM ============================
    /* This system spawned plants by first creating spawner objects
     * at random positions on the spawner grid. Then it would spawn
     * enemies from each of the spawner objects.
     */

    // Spawn a burst of enemies from all sources
    /*private void BurstSpawn(string currnetPhase)
    {
        for (int i = 0; i < spawnArray.Length; i++)
        {
            Spawner currentSpawner = spawnArray[i].GetComponent<Spawner>();
            if (currentSpawner.germinationPeriod == currnetPhase)
            {
                currentSpawner.Spawn("Full");
                currentSpawner.lastTimeSpawned = elapsedTime;
            }
        }
    }*/

    // OLD -> Used to create spawner objects on the grid randomly
    private void CreateSpawnersonGrid(int num, string phaseWeIn)
    {
        Vector3[] positions = spawnerGridPositions.Keys.ToArray();
        bool validPos = false;

        for (int i = 0; i < num; i++)
        {
            // PICK RANDOM SPOT IN DICTIONARY
            Vector3 pos = new Vector3(0, 0, 0);
            if (positions.Length > 0)
                pos = positions[Random.Range(0, positions.Length - 1)];
            else
                Debug.Log("Dictionary has a length of: " + positions.Length);

            string availablePos;
            // TEST IF POSITION IS FILLED, UPDATE DICTIONARY
            if (spawnerGridPositions.TryGetValue(pos, out availablePos))
            {
                // Success
                if (availablePos == "Empty")
                {
                    spawnerGridPositions[pos] = "Full";
                    validPos = true;
                }
                else
                {
                    Debug.Log("Position Taken");
                    validPos = false;
                    i--;
                }
            }
            else
                Debug.LogError("Dictionary error");

            if (validPos)
            {
                GameObject selectedEnemy = null;
                float rand = Random.Range(1, 10);
                //Debug.Log(rand);
                if (rand < 6) selectedEnemy = meleeEnemy;
                else selectedEnemy = rangeEnemy;

                GameObject newSpawner = Instantiate(selectedEnemy, pos, transform.rotation);
                //alignToGrid(newSpawner.transform);
                spawners.Add(newSpawner);
            }
        }
    }
}

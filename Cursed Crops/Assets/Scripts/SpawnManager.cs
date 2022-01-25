using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    // Public Variables
    [Header("Each phase has X waves and X-1 puases. (There is no pause after the last wave)")]
    public float phaseDuration = 150f;      // Duration of each phase in seconds
    public float waveDuration = 30f;        // Duration of each wave in seconds
    public float pauseDuration = 10f;       // Duration of each short pause between waves in seconds
    public int wavesPerPhase = 4;           // The # of waves in each phase. Make sure the time values add up correctly

    public float elapsedTime;               // Time since level started
    public string currentPhase = "Pre";     // Stars in Pre then goes to Morning, Afternoon and Night
    public int currentWave = 0;             // Increments up for each wave in the phase

    public GameObject harvestStartFlag;     // Some object that, when each player interacts with it, begins the harvest.
    private GameObject harvestFlag;
    public GameObject placementChecker;     // A prefab used to check if a location is suitable for a crop spawn
    public GameObject morningBasicEnemy;    // The basic enemy to spawn in Morning phase
    public GameObject afternoonBasicEnemy;  // The basic enemy to spawn in Afternoon phase
    public GameObject nightBasicEnemy;      // The basic enemy to spawn in Night phase

    public float bounds = 18f;

    public float gridSize = 1;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    // Private Variables
    private float currentPhaseEndTime = 0f;  // The time the current phase should end
    private float currentWaveEndTime = 0f;   // The time the current wave should end
    private float currentPauseEndTime = 0f;  // The time the current pause should end
    private bool timerStarted = false;
    private bool gridUpdated = false;
    private delegate void Callback();
    public enum State
    {
        Wave,
        Pause,
        Break
    }
    public State state;

    // For rn spawner works with an array. But change to a list when planting phase is in
    // Because it will be easier to dynamically add and remove to
    public List<GameObject> spawners = new List<GameObject>();
    //public GameObject[] spawnArray;

    public GameObject gridChild;
    private Dictionary<Vector3, string> spawnerGridPositions = new Dictionary<Vector3, string>();

    private void Start()
    {
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

        Debug.Log("There are " + gridChild.transform.childCount + " grid children");

        if (gridUpdated)
            RunSpawnSystem();
    }

    private void RunSpawnSystem()
    {
        // Phase timer
        // INSERT: Starting wave only when players ready
        if (currentPhaseEndTime < elapsedTime)
        {
            currentPhaseEndTime = elapsedTime + phaseDuration;

            if (currentPhase == "Pre")
            {
                currentPhase = "Morning";
                state = State.Break;
                harvestFlag.SetActive(true);
                CreateSpawnersonGrid(4, "Morning");
            }
            else if (currentPhase == "Morning")
            {
                StopAllCoroutines();
                currentPhase = "Afternoon";
                state = State.Break;
                harvestFlag.SetActive(true);
                DestroySpawners();
                CreateSpawnersonGrid(4, "Afternoon");
            }
            else if (currentPhase == "Afternoon")
            {
                StopAllCoroutines();
                currentPhase = "Night";
                state = State.Break;
                harvestFlag.SetActive(true);
                DestroySpawners();
                CreateSpawnersonGrid(4, "Night");
            }
            else if (currentPhase == "Night")
            {
                StopAllCoroutines();
                currentPhase = "Post";
                state = State.Break;
                DestroySpawners();
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
        if (state == State.Wave)
            WaveSpawn("Full");
    }

    public void StartHarvest()
    {
        state = State.Wave;
        harvestFlag.SetActive(false);
    }

    IEnumerator timer(Callback callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    public void AddSpawner(GameObject spawner)
    {
        spawners.Add(spawner);
    }

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

    // Spawn enemies as the wave progresses. Time between spawns is measured by a spawners potency
    private void WaveSpawn(string type)
    {
        foreach (GameObject obj in spawners)
        {
            Spawner currentSpawner = obj.GetComponent<Spawner>();
            if (currentSpawner.lastTimeSpawned == 0) // Initial spawn
            {
                currentSpawner.Spawn(type);
                //currentSpawner.lastTimeSpawned = elapsedTime;
                // This version makes spawns more diverse
                currentSpawner.lastTimeSpawned = elapsedTime + Random.Range(0f, 5f);
            }
            else if ((currentSpawner.lastTimeSpawned + currentSpawner.potency) <= elapsedTime)
            {
                currentSpawner.Spawn(type);
                currentSpawner.lastTimeSpawned = elapsedTime;
            }
        }
    }

    private void CreateSpawnerGrid()
    {
        for(float i = -bounds; i <= bounds-1; i++)
        {
            for (float j = -bounds; j <= bounds-1; j++)
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
            /*if (gridChild.transform.GetChild(i).GetComponent<GridPlacementChecker>().acceptablePos)
            {
                spawnerGridPositions.Add(gridChild.transform.GetChild(i).transform.position, "Empty");
                gridChild.transform.GetChild(i).gameObject.SetActive(false);
            }
            else if (!gridChild.transform.GetChild(i).GetComponent<GridPlacementChecker>().acceptablePos)
            {
                Debug.Log("Unacceptable position, deleting");
                Destroy(gridChild.transform.GetChild(i).gameObject);
            }*/
        }
        //Debug.Log("The dictionary has " + spawnerGridPositions.Count + " entires.");
        /*foreach (KeyValuePair<Vector3, string> pos in spawnerGridPositions)
        {
            Debug.Log("Position: " + pos.Key.ToString() + " is " + pos.Value);
        }*/
    }

    private void CreateSpawnersonGrid(int num, string phaseWeIn)
    {
        Vector3[] positions = spawnerGridPositions.Keys.ToArray();
        bool validPos = false;

        for (int i = 0; i < num; i++)
        {
            // PICK RANDOM SPOT IN DICTIONARY
            Vector3 pos = new Vector3(0,0,0);
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
                } else
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
                if (phaseWeIn == "Morning")
                    selectedEnemy = morningBasicEnemy;
                else if (phaseWeIn == "Afternoon")
                    selectedEnemy = afternoonBasicEnemy;
                else if (phaseWeIn == "Night")
                    selectedEnemy = nightBasicEnemy;

                GameObject newSpawner = Instantiate(selectedEnemy, pos, transform.rotation);
                //alignToGrid(newSpawner.transform);
                spawners.Add(newSpawner);
            }
        }
    }

    private void DestroySpawners()
    {
        foreach (GameObject obj in spawners)
        {
            Destroy(obj);
        }

        spawners.Clear();
    }

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

    private void OnDisable()
    {
        //Debug.Log("Called disable from spawnmanager");
        //spawnerGridPositions.Clear();
    }
}

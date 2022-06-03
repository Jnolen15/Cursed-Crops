using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class EnemyTalking : MonoBehaviour
{

    //Variables that will change the character picture, name, etc
    public GameObject spawnerObject;
    public GameObject daName;
    public GameObject image;
    public GameObject daDialogue;
    public GameObject houseArrow;
    public GameObject playerHealthArrow;
    public GameObject QuotaArrow;
    
    //Quest Tracker Variables
    public GameObject QuestTracker;
    public GameObject talkTo;
    public GameObject combatList;
    public GameObject AttackDone;
    public GameObject RollDone;
    public GameObject ShootDone;
    public GameObject doDamage;

    //Controls Screen Variables
    public GameObject controls;
    public GameObject combatControls;
    public GameObject plantingControls;
    public GameObject upgradeControls;
    public GameObject quotaStuff;
    public GameObject Move_Interact;

    //Getting Broc Sprite
    public Sprite Broc_Bro;


    //Variables for calling the Dialogue Class
    public string sentence;
    public string waitingSentence;
    public string character;
    public string DaEvent;
    public bool stopDialogue;
    public bool enemyAction;
    public bool dialogueHappening;
    public bool startingDial;
    public bool taskFinish = false;
    public bool trySomething = false;
    public bool interactOnce = false;
    public bool wakingScene = true;
    public bool endOfTutorial = false;

    public bool textOver = false;

    public DialogueClass[] dialogue;
    public GameObject[] players;
    private GameObject firstPlayer;
    private GameObject[] allPlayers;
    public GameObject dialogueBox;
    //public PlayerInputHandler pc;

    private Queue<string> sentences;
    private Queue<string> characters;
    private Queue<string> DaEvents;
    private Queue<bool> stoppers;
    private Queue<bool> actions;
    private Queue<bool> starting;
    private Transform originalLocal;
    private bool talking = false;
    private bool stopSpawning = false;
    private bool giveMoneyOnce = false;
    private bool showBox = false;
    private bool placeIsInWorld = false;
    private bool aTurretIsDestroyed = false;
    private bool aSeedIsDestroyed = false;
    private GameObject drop;
    private GameObject seed;
    private GameObject turret;

    private GameObject childOfSpawner;

    private GameRuleManager grm;
    private UpgradeManager grmupgrade;

    void Awake()
    {
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
        grmupgrade = GameObject.Find("GameRuleManager").GetComponent<UpgradeManager>();
        sentences = new Queue<string>();
        characters = new Queue<string>();
        DaEvents = new Queue<string>();
        stoppers = new Queue<bool>();
        actions = new Queue<bool>();
        starting = new Queue<bool>();
        sentences.Clear();
        characters.Clear();
        stoppers.Clear();

        // Goes through all the lines of dialogue with who is speaking
        // their line and if they end the dialogue sequence
        foreach (DialogueClass line in dialogue)
        {


            //sentences.Enqueue(sentence);

            stoppers.Enqueue(line.endOfDialogue);
            actions.Enqueue(line.enemyAttack);
            starting.Enqueue(line.endOfStartingDialogue);
            characters.Enqueue(line.name);
            DaEvents.Enqueue(line.eventWeWant);
            sentences.Enqueue(line.sentences);
            //line.endOfDialogue = false;

        }
        childOfSpawner = spawnerObject.transform.GetChild(0).gameObject;
        firstPlayer = GameObject.FindGameObjectWithTag("Player");
        players = GameObject.FindGameObjectsWithTag("Player");

        allPlayers = GameObject.FindGameObjectsWithTag("PlayerControlTag");
        
        
        DisplayNextSentence();
        //dialogueBox.SetActive(true);
        dialogueBox.SetActive(true);
        talking = true;
        

    }

    public void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        firstPlayer = GameObject.FindGameObjectWithTag("Player");
        allPlayers = GameObject.FindGameObjectsWithTag("PlayerControlTag");
        
        // if any player goes and talks it will start dialogue and disable player inputs
        foreach (GameObject player in players)
        {

            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 5)
            {
                
                gameObject.GetComponent<EnemyToPlayer>().closestPlayer = player.transform;
                if (allPlayers != null)
                {
                    foreach (GameObject control in allPlayers)
                    {
                        control.GetComponent<PlayerInputHandler>().dialogueIsHappening = true;
                        if (control.GetComponent<PlayerInputHandler>().forDialogue && !enemyAction && !trySomething && !endOfTutorial)
                        {
                            Debug.Log("hello");
                            talking = true;

                        }
                        else if (control.GetComponent<PlayerInputHandler>().forDialogue && taskFinish)
                        {
                            talking = true;

                        }
                    }

                }
            }
            /*
            else if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 20 && !wakingScene)
            {
                talking = false;
                //pc.dialogueIsHappening = false;
                foreach (GameObject control in allPlayers)
                {
                    control.GetComponent<PlayerInputHandler>().forDialogue = false;
                    control.GetComponent<PlayerInputHandler>().dialogueIsHappening = false;
                }
            }
            */

        }

        // This will stay in update so that the text box can show and be turned off as well
        if (allPlayers != null)
        {
            //Debug.Log(pc.forDialogue);
            foreach (GameObject control in allPlayers)
            {
                if (wakingScene)
                {
                    control.GetComponent<PlayerInputHandler>().allowBuild = false;
                    control.GetComponent<PlayerInputHandler>().allowAttack = false;
                }
                if (talking)
                {
                    QuestTracker.SetActive(false);
                    talkTo.SetActive(false);
                    if (!showBox)
                    {
                        controls.SetActive(false);
                    }
                    Move_Interact.SetActive(false);
                    control.GetComponent<PlayerInputHandler>().dialogueIsHappening = true;
                    //disable players controller for the meantime
                    foreach (GameObject player in players)
                    {
                        player.GetComponent<PlayerControler>().animator.SetFloat("MovementMagnitude", 0);
                        player.GetComponent<PlayerControler>().forDialogue = true;
                        player.GetComponent<PlayerControler>().enabled = false;
                    }

                    //Jared plz help me with getting the cotrols :(
                    //if (pc.state == PlayerControler.State.Attacking && !interactOnce && !enemyAction)
                    if (control.GetComponent<PlayerInputHandler>().forDialogue && !enemyAction && !trySomething)
                    {

                        dialogueBox.SetActive(true);
                        control.GetComponent<PlayerInputHandler>().forDialogue = false;

                        if (textOver)
                        {


                            if (!stopDialogue)
                            {
                                DisplayNextSentence();

                            }
                            else
                            {
                                talking = false;
                            }

                        }
                        else
                        {

                            SkipAnimation();


                        }


                    }
                    else if (control.GetComponent<PlayerInputHandler>().forDialogue && taskFinish)
                    {
                        dialogueBox.SetActive(true);
                        control.GetComponent<PlayerInputHandler>().forDialogue = false;

                        if (textOver)
                        {


                            if (!stopDialogue)
                            {
                                DisplayNextSentence();

                            }
                            else
                            {
                                talking = false;
                            }

                        }
                        else
                        {

                            SkipAnimation();


                        }
                    }

                }
                else //Once a certain amount of dialogue is said we return player control again and disable the text box
                {


                    foreach (GameObject player in players)
                    {
                        if (!enemyAction)
                        {
                            player.GetComponent<PlayerControler>().enabled = true;
                            player.GetComponent<PlayerControler>().forDialogue = false;
                        }
                    }
                    stopDialogue = false;
                    
                    control.GetComponent<PlayerInputHandler>().forDialogue = false;
                    control.GetComponent<PlayerInputHandler>().dialogueIsHappening = false;
                    
                    dialogueBox.SetActive(false);


                }
            }
            // This is just for the starting dialogue when entering the tutorial
            if (startingDial)
            {
                wakingScene = false;
                
            }


             // This code will be use for the different interactions that need to be done
             switch (DaEvent)
             {
                case "Talk":
                    if (!dialogueBox.activeSelf)
                    {
                        QuestTracker.SetActive(true);
                        talkTo.SetActive(true);
                        controls.SetActive(true);
                        Move_Interact.SetActive(true);
                    }
                    break;
                case "Enemy Attack":
                    
                    gameObject.tag = "Enemy";
                    gameObject.GetComponent<WindupWithStun>().enabled = true;
                    
                    break;
                case "House Health":
                    houseArrow.SetActive(true);    
                    break;
                case "Player Health":
                    playerHealthArrow.SetActive(true);
                    break;
                case "Quota Arrow":
                    QuotaArrow.SetActive(true);
                    break;
                case "No Arrow":
                    houseArrow.SetActive(false);
                    playerHealthArrow.SetActive(false);
                    QuotaArrow.SetActive(false);
                    childOfSpawner.GetComponent<SphereCollider>().enabled = false;
                    
                    break;
                case "Show Dodge":
                    houseArrow.SetActive(false);
                    showBox = true;
                    controls.SetActive(true);
                    combatControls.SetActive(true);
                    combatControls.GetComponent<TextMeshProUGUI>().text = "Dodge: While moving press Shift/Right Bumper";
                    break;
                case "Show Attack":
                    combatControls.GetComponent<TextMeshProUGUI>().text = "Attack: Press Left Click/Right Trigger Button.\nDodge: While moving press Shift/Right Bumper";
                    break;
                case "Try Attacking":
                    combatControls.GetComponent<TextMeshProUGUI>().text = "Shoot: Press Right Click/Left Trigger.\nAttack: Press Left Click/Right Trigger Button.\nDodge: While moving press Shift/Right Bumper";
                    if (!dialogueBox.activeSelf)
                    {
                        QuestTracker.SetActive(true);
                        combatList.SetActive(true);
                        controls.SetActive(true);
                        combatControls.SetActive(true);
                        trySomething = true;
                        gameObject.GetComponent<CapsuleCollider>().enabled = true;
                        foreach (GameObject control in allPlayers)
                        {
                            control.GetComponent<PlayerInputHandler>().allowAttack = true;
                        }
                    }

                    if(gameObject.GetComponent<EnemyControler>().health <= 9980)
                    {
                        doDamage.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    }
                    if (checkPlayersDodge())
                    {
                        RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    }
                    foreach (GameObject control in allPlayers)
                    {
                        if (control.GetComponent<PlayerInputHandler>().attackOnce && control.GetComponent<PlayerInputHandler>().shootOnce && checkPlayersDodge() && gameObject.GetComponent<EnemyControler>().health <= 9980 && gameObject.GetComponent<CapsuleCollider>().enabled)
                        {
                            trySomething = false;
                            gameObject.GetComponent<CapsuleCollider>().enabled = false;
                            combatList.SetActive(false);
                            talkTo.SetActive(true);
                            combatControls.SetActive(false);
                            controls.SetActive(false);
                            showBox = false;
                        }
                        if (control.GetComponent<PlayerInputHandler>().attackOnce)
                        {
                            AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                        }
                        if (control.GetComponent<PlayerInputHandler>().shootOnce)
                        {
                            ShootDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                        }
                        

                    }

                    break;

                case "Do a Planting Phase":
                    if (!dialogueBox.activeSelf)
                    {
                        QuestTracker.SetActive(true);
                        combatList.SetActive(true);
                        controls.SetActive(true);
                        plantingControls.SetActive(true);
                        trySomething = true;
                    }
                    AttackDone.GetComponent<TextMeshProUGUI>().text = "Buy an upgrade at the house.";
                    AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    RollDone.GetComponent<TextMeshProUGUI>().text = "Build a turret on the grass.";
                    RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    ShootDone.GetComponent<TextMeshProUGUI>().text = "Plant some crops in the field.";
                    ShootDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    doDamage.GetComponent<TextMeshProUGUI>().text = "";

                    if (grm.getMoney() < 100 && (!grmupgrade.updgradeBought || turret == null))
                    {
                        grm.addMoney(100);
                    }

                    foreach (GameObject control in allPlayers)
                    {
                        control.GetComponent<PlayerInputHandler>().allowBuild = true;
                    }
                    taskFinish = true;
                    seed = GameObject.FindGameObjectWithTag("Spawner");
                    turret = GameObject.FindGameObjectWithTag("Turret");
                    if(seed != null)
                    {
                        ShootDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    }
                    if(turret != null)
                    {
                        RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    }
                    if (grmupgrade.updgradeBought)
                    {
                        AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    }
                    waitingSentence = "I saw you writing that all down. Crack that menu open and get to work!";
                    break;

                case "Waiting to Start Harvest":
                    if (!dialogueBox.activeSelf)
                    {
                        QuestTracker.SetActive(true);
                        combatList.SetActive(true);
                        controls.SetActive(true);
                        quotaStuff.SetActive(true);
                        trySomething = true;
                    }
                    AttackDone.GetComponent<TextMeshProUGUI>().text = "Reach The Quota";
                    AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    RollDone.GetComponent<TextMeshProUGUI>().text = "Stand On Flag To Start";
                    RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    taskFinish = true;
                    spawnerObject.GetComponent<SpawnManager>().enabled = false;
                    childOfSpawner.GetComponent<SphereCollider>().enabled = true;
                    if (grm.bountyMet(0.2f))
                    {
                        AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                    }
                    waitingSentence = "Start the harvest phase whenever you’re ready. Big red flag. By the farmhouse. Can’t miss it.";
                    break;

                case "Buyer Remorse":
                    if (!dialogueBox.activeSelf)
                    {
                        QuestTracker.SetActive(true);
                        combatList.SetActive(true);
                        controls.SetActive(true);
                        upgradeControls.SetActive(true);
                        trySomething = true;
                    }
                    AttackDone.GetComponent<TextMeshProUGUI>().text = "Destroy a Turret";
                    AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    RollDone.GetComponent<TextMeshProUGUI>().text = "Un-Plant a Seed";
                    RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                    ShootDone.GetComponent<TextMeshProUGUI>().text = "";
                    doDamage.GetComponent<TextMeshProUGUI>().text = "";

                    foreach (GameObject player in players)
                    {
                        if (player.GetComponent<BuildingSystem>().turretDestroy)
                        {
                            aTurretIsDestroyed = true;
                            AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;

                        }
                        if (player.GetComponent<BuildingSystem>().seedDestroy)
                        {
                            aSeedIsDestroyed = true;
                            RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                        }
                    }



                    break;

                case "Drop Items":
                    if (!dialogueBox.activeSelf)
                    {
                        QuestTracker.SetActive(true);
                        

                    }
                    if (!stopSpawning)
                    {
                        stopSpawning = true;
                        if (!dialogueBox.activeSelf)
                        {
                            trySomething = true;
                        }
                        combatList.SetActive(true);
                        AttackDone.GetComponent<TextMeshProUGUI>().text = "Pick Up Crop";
                        AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                        RollDone.GetComponent<TextMeshProUGUI>().text = "Take Crop to Farmhouse";
                        RollDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
                        ShootDone.GetComponent<TextMeshProUGUI>().text = "";
                        doDamage.GetComponent<TextMeshProUGUI>().text = "";
                        Vector3 position = new Vector3(gameObject.transform.position.x, 1, gameObject.transform.position.z - 2);
                        gameObject.GetComponent<ItemDropper>().DropItem(position);
                        drop = GameObject.FindGameObjectWithTag("DroppedItem");
                        drop.GetComponent<ItemDrop>().destroyTime = 1000;
                        taskFinish = true;
                        if (drop != null)
                        {
                            waitingSentence = "Don't be afraid! Go on, pick it up!";
                        }
                        
                        

                    }
                    
                    break;

                case "End of Tutorial":
                    if (!dialogueBox.activeSelf)
                    {
                        endOfTutorial = true;
                        gameObject.GetComponent<EnemyControler>().health = 1;
                        
                    }
                    break;



             }
             if (gameObject.GetComponent<WindupWithStun>().windupFinish)
             {
                 gameObject.GetComponent<WindupWithStun>().enabled = false;
                 gameObject.GetComponent<WindupWithStun>().sr.color = gameObject.GetComponent<WindupWithStun>().prev;
                 gameObject.GetComponent<EnemyToPlayer>().enemySpeed = gameObject.GetComponent<EnemyToPlayer>().originalSpeed;

                 enemyAction = false;
             }


            
             //Checking if player got the crops in their inventory and taking it to the farmhouse
            if (drop == null && taskFinish && DaEvent == "Drop Items")
            {
                AttackDone.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                waitingSentence = "Great! Why don't you exchange it for money at the farmhouse";

                trySomething = true;
                taskFinish = true;
                if (checkPlayersCropAmount())
                {
                    trySomething = false;
                    taskFinish = false;
                    combatList.SetActive(false);
                    talkTo.SetActive(true);

                }
                

            }
            if(seed != null && turret != null && grmupgrade.updgradeBought && taskFinish && DaEvent == "Do a Planting Phase")
            {
                combatList.SetActive(false);
                talkTo.SetActive(true);
                plantingControls.SetActive(false);
                controls.SetActive(false);
                trySomething = false;
                taskFinish = false;
            }
            if(DaEvent == "Buyer Remorse" && aSeedIsDestroyed && aTurretIsDestroyed)
            {
                combatList.SetActive(false);
                talkTo.SetActive(true);
                upgradeControls.SetActive(false);
                controls.SetActive(false);
                trySomething = false;
                taskFinish = false;

            }
/*
            if (placeble != null && placeble.tag == "Turret" && taskFinish && DaEvent == "Place Turret")
            {
                combatList.SetActive(false);
                talkTo.SetActive(true);
                turretControls.SetActive(false);
                controls.SetActive(false);
                trySomething = false;
                taskFinish = false;
            }
*/
            if (DaEvent == "Buy An Upgrade" && taskFinish && grmupgrade.updgradeBought)
            {
                combatList.SetActive(false);
                talkTo.SetActive(true);
                upgradeControls.SetActive(false);
                controls.SetActive(false);
                trySomething = false;
                taskFinish = false;

            }
            if (!childOfSpawner.activeSelf && DaEvent == "Waiting to Start Harvest")
            {
                trySomething = false;
                taskFinish = false;
                startingDial = false;
                wakingScene = true;
                talking = true;
                quotaStuff.SetActive(false);
                controls.SetActive(false);
                dialogueBox.SetActive(true);
                DisplayNextSentence();

            }
            
            if (endOfTutorial)
            {
                spawnerObject.GetComponent<SpawnManager>().enabled = true;
                gameObject.GetComponent<CapsuleCollider>().enabled = true;
                Vector3 endPosition = new Vector3(-18.51F, 1F, -9.27F);
                transform.position = Vector3.MoveTowards(gameObject.transform.position, endPosition, 0.01F);
                if (gameObject.transform.position == endPosition)
                {
                    gameObject.SetActive(false);
                }
                foreach (GameObject control in allPlayers)
                {
                    control.GetComponent<PlayerInputHandler>().allowBuild = true;
                    control.GetComponent<PlayerInputHandler>().allowAttack = true;
                }

            }

        }




    }

    private bool checkPlayersCropAmount()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i].GetComponent<PlayerResourceManager>().getCrops() != 0)
            {
                return false;
                
                
            }

        }
        return true;
    }


    private bool checkPlayersDodge()
    {
        for (int i = 0; i < players.Length; ++i)
        {
            if (players[i].GetComponent<PlayerControler>().rollDone)
            {
                return true;


            }

        }
        return false;
    }
    public void DisplayNextSentence()
    {
        if (!taskFinish)
        {
            if (sentences.Count == 0 || characters.Count == 0)
            {
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerControler>().enabled = true;
                }
                dialogueBox.SetActive(false);
                talking = false;
                Debug.Log("reach the end of the dialoge m8");
                return;
            }
            sentence = sentences.Dequeue();
            stopDialogue = stoppers.Dequeue();
            enemyAction = actions.Dequeue();
            startingDial = starting.Dequeue();
            daName.GetComponent<TextMeshProUGUI>().text = characters.Dequeue();
            DaEvent = DaEvents.Dequeue();
            switch (daName.GetComponent<TextMeshProUGUI>().text)
            {

                case "Broc-Broc":
                    image.GetComponent<Image>().sprite = Broc_Bro;
                    break;

            }
        }
        else
        {
            sentence = waitingSentence;
            //daName.GetComponent<TextMeshProUGUI>().text = "Broc-Bro";
            //image.GetComponent<Image>().sprite = Broc_Bro;
            stopDialogue = true;
        }
            
            
            

            

            //Deals with the images of the dialogue

            

            

            //daDialogue.GetComponent<TextMeshProUGUI>().text = sentences.Dequeue();
            
        
        textOver = false;
        
        StopAllCoroutines();
        StartCoroutine(TypeSenctence(sentence));
        //interactOnce = true;
        //Use this space to add the letter animations - Juan

    }

    IEnumerator TypeSenctence(string sentence)
    {
        
        daDialogue.GetComponent<TextMeshProUGUI>().text = "";
        int count = 0;
        foreach (char letter in sentence.ToCharArray())
        {
            count++;
            daDialogue.GetComponent<TextMeshProUGUI>().text += letter;
            yield return new WaitForSeconds(0.03f);
            /*if (count % 5 == 0)
            {
                if (Random.value < 0.5f)
                    audioManager.source.PlayOneShot(audioManager.Talk1);
                else
                    audioManager.source.PlayOneShot(audioManager.Talk2);
            }*/
        }
        interactOnce = false;
        textOver = true;
        
    }

    public void SkipAnimation()
    {
        StopAllCoroutines();
        daDialogue.GetComponent<TextMeshProUGUI>().text = sentence;
        textOver = true;
        //interactOnce = false;
    }
}

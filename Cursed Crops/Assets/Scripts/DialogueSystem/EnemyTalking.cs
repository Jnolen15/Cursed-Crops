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
    public GameObject dialogueBox;
    public PlayerControler pc;

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
    private bool placeIsInWorld = false;
    private GameObject drop;
    private GameObject placeble;
    private GameObject childOfSpawner;

    private GameRuleManager grm;

    void Awake()
    {
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
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
        
        
        if (firstPlayer != null)
        {
            pc = firstPlayer.GetComponent<PlayerControler>();
        }
        DisplayNextSentence();
        //dialogueBox.SetActive(true);
        dialogueBox.SetActive(true);
        talking = true;
        
    }

    public void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        firstPlayer = GameObject.FindGameObjectWithTag("Player");
        
        pc = firstPlayer.GetComponent<PlayerControler>();
        // if any player goes and talks it will start dialogue and disable player inputs
        foreach (GameObject player in players)
        {

            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 5)
            {

                gameObject.GetComponent<EnemyToPlayer>().closestPlayer = player.transform;

                if (pc.forDialogue && !enemyAction && !trySomething && !endOfTutorial)
                {
                    Debug.Log("hello");
                    talking = true;
                }
                else if(pc.forDialogue && taskFinish)
                {
                    talking = true;
                }


            }
            else if (Vector3.Distance(gameObject.transform.position, player.transform.position) > 20 && !wakingScene)
            {
                talking = false;
            }

        }

        // This will stay in update so that the text box can show and be turned off as well
        if (pc != null) {
            Debug.Log(pc.forDialogue);
            if (talking)
            {
                
                //disable players controller for the meantime
                foreach (GameObject player in players)
                {
                    player.GetComponent<PlayerControler>().enabled = false;
                }

                //Jared plz help me with getting the cotrols :(
                //if (pc.state == PlayerControler.State.Attacking && !interactOnce && !enemyAction)
                if (pc.forDialogue && !enemyAction && !trySomething)
                {
                    
                    dialogueBox.SetActive(true);
                    pc.forDialogue = false;

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
                else if(pc.forDialogue && taskFinish)
                {
                    dialogueBox.SetActive(true);
                    pc.forDialogue = false;

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
                    }
                }

                stopDialogue = false;
                dialogueBox.SetActive(false);


            }

            // This is just for the starting dialogue when entering the tutorial
            if (startingDial)
            {
                wakingScene = false;
            }


             // This code will be use for the different interactions that need to be done
             switch (DaEvent)
             {
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
                case "No Arrow":
                    houseArrow.SetActive(false);
                    playerHealthArrow.SetActive(false);
                    childOfSpawner.GetComponent<SphereCollider>().enabled = false;
                    
                    break;
                case "Try Attacking":
                    if (!dialogueBox.activeSelf)
                    {
                        trySomething = true;
                        gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    }
                    if (!dialogueBox.activeSelf)
                    {
                        trySomething = true;
                        
                    }
                    break;

                case "Plant Seed":
                    if (!dialogueBox.activeSelf)
                    {
                        trySomething = true;
                    }
                    taskFinish = true;
                    placeble = GameObject.FindGameObjectWithTag("Spawner");
                    waitingSentence = "Remember, you can plant the seed by pressing R / North Button to open the seed menu and look through the selections using Q and E / Left or Right bumpers, once you decide hit the good ol' Left Click/South Button to plant it";
                    break;

                case "Place Turret":
                    if (!dialogueBox.activeSelf)
                    {
                        trySomething = true;
                    }
                    if (grm.getMoney() < 100)
                    {
                        grm.addMoney(100);
                    }
                    taskFinish = true;
                    placeble = GameObject.FindGameObjectWithTag("Turret");
                    waitingSentence = "Remember, you can place a turret by pressing the R / North Button to open the shop menu and look through the selections using Q and E / Left or Right bumpers, once you decide hit the good ol' Left Click/South Button to place it";
                    break;

                case "Waiting to Start Harvest":
                    if (!dialogueBox.activeSelf)
                    {
                        trySomething = true;
                    }
                    taskFinish = true;
                    spawnerObject.GetComponent<SpawnManager>().enabled = false;
                    childOfSpawner.GetComponent<SphereCollider>().enabled = true;
                    waitingSentence = "Hey farmer buddy, I know you like talking to me but you know you gotta start the harvest phase to finish the tutorial right?";
                    break;

                case "Buy An Upgrade":
                    if (!dialogueBox.activeSelf)
                    {
                        trySomething = true;
                    }
                    taskFinish = true;
                    waitingSentence = "Remember just do the same process to buy a turret on top of the tan ground";
                    break;

                case "Give Money":
                    if (!giveMoneyOnce)
                    {
                        giveMoneyOnce = true;
                        grm.addMoney(100);
                    }
                    
                    break;
                case "Drop Items":
                    if (!stopSpawning)
                    {
                        stopSpawning = true;
                        if (!dialogueBox.activeSelf)
                        {
                            trySomething = true;
                        }
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
             
            if(gameObject.GetComponent<EnemyControler>().health <= 9980 && gameObject.GetComponent<CapsuleCollider>().enabled)
            {
                trySomething = false;
                gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }

            if (drop == null && taskFinish && DaEvent == "Drop Items")
            {
                waitingSentence = "Great! Why don't you exchange it for money at the farmhouse";
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PlayerResourceManager>().getCrops() != 0)
                    {
                        trySomething = true;
                        taskFinish = true;
                    }
                    else
                    {
                        trySomething = false;
                        taskFinish = false;
                    }
                }
            }
            if(placeble != null && placeble.tag == "Spawner" && taskFinish && DaEvent == "Plant Seed")
            {
                trySomething = false;
                taskFinish = false;
            }

            if (placeble != null && placeble.tag == "Turret" && taskFinish && DaEvent == "Place Turret")
            {
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
                dialogueBox.SetActive(true);
                DisplayNextSentence();

            }
            if(DaEvent == "Buy An Upgrade" && taskFinish)
            {
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<BuildingSystem>().updgradeBought)
                    {
                        trySomething = false;
                        taskFinish = false;
                    }
                }
            }
            if (endOfTutorial)
            {
                spawnerObject.GetComponent<SpawnManager>().enabled = true;
                gameObject.GetComponent<CapsuleCollider>().enabled = true;
                Vector3 endPosition = new Vector3(-18.51F, 1F, -9.27F);
                transform.position = Vector3.MoveTowards(gameObject.transform.position, endPosition, 0.01F);
                if(gameObject.transform.position == endPosition)
                {
                    gameObject.SetActive(false);
                }
            }


        }




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

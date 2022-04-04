using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    // ================= Private variables =================
    [SerializeField] private popUpUISO popupUI;
    [SerializeField] private statShopSO statShopUI;
    private GameObject popUp;
    private GameObject statShop;
    private PlantingUIManager popUpMan;
    private StatShopUIManager statShopMan;
    private GameRuleManager grm;
    private PlayerControler pc;
    private BuildChecker bc;
    private SpawnManager sm;
    private Animator animator;
    private ParticleSystem ps;
    private ParticleSystem psDust;
    private ParticleSystem psUpgrade;
    private bool acceptablePos;
    private int count = 0;
    private float xPos = 1f;
    private float zPos = 0f;
    private Vector3 prevDir;

    // ================= Public variables =================
    public PlaceableSO activePlaceable;
    public CropSO activeCrop;
    public GameObject placeableHighlight;
    public GameObject phSprite;
    public SpriteRenderer pHSpriteRenderer;
    public bool buildmodeActive = false;
    public string mode = "Build";
    public float gridSize = 1;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    public int upgradeCost = 50;
    public int healthUpgrade = 3;
    public float damageUpgrade = 0.5f;
    public float speedUpgrade = 1;
    public int carryUpgrade = 10;
    public EnemyPlayerDamage EPD;
    public AudioClip plantingSound;
    public AudioClip buildingSound;




    private void Start()
    {
        //placeableHighlight = this.transform.GetChild(4).gameObject;
        //phSprite = placeableHighlight.transform.GetChild(0).gameObject;
        //pHSpriteRenderer = phSprite.GetComponent<SpriteRenderer>();
        pc = this.GetComponent<PlayerControler>();
        bc = placeableHighlight.GetComponent<BuildChecker>();
        sm = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        animator = this.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Animator>();
        ps = placeableHighlight.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        ps.Pause();
        popUp = Instantiate(popupUI.Prefab.gameObject,new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 
                            transform.rotation, transform);
        statShop = Instantiate(statShopUI.Prefab.gameObject, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
                            transform.rotation, transform);

        statShopMan = statShop.GetComponent<StatShopUIManager>();
        popUpMan = popUp.GetComponent<PlantingUIManager>();
        //popUp.SetActive(false); MOVED TO IN POPUP
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
        // Set default selected placeable
        if (popupUI.buildables.Length > 0 && popupUI.buildables.Length > 0)
        {
            activePlaceable = popupUI.buildables[0];
            activeCrop = popupUI.plantables[0];
        } else
        {
            Debug.Log("Placeable and/or Plantable array has a size of 0");
        }

        psDust = Instantiate(Resources.Load<GameObject>("Effects/DustParticle"), placeableHighlight.transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        psUpgrade = Instantiate(Resources.Load<GameObject>("Effects/UpgradeParticle"), transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        psDust.Pause();
        psUpgrade.Pause();

        buildmodeActive = false;
        mode = "Build";
        placeableHighlight.SetActive(false);
        Debug.Log(this.gameObject.name);

        EPD = GetComponent<EnemyPlayerDamage>();
    }


    private void Update()
    {
        // Un-ready the player once a round starts (I added this here becuase it already had a SM refrence)
        if(sm.state == SpawnManager.State.Wave && pc.ready)
            pc.ready = false;
        
        // Update buld preview
        if (buildmodeActive)
        {
            AlignToGrid(placeableHighlight.transform);
            AlignToGrid(popUp.transform);
            AlignToGrid(psDust.transform);
            TestPlacementAvailable();

            // update preview
            if (mode == "Build" && pHSpriteRenderer.sprite != activePlaceable.preview)
            {
                pHSpriteRenderer.sprite = activePlaceable.preview;
                phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                ps.Play();
            } else if (mode == "Plant" && pHSpriteRenderer.sprite != activePlaceable.preview)
            {
                pHSpriteRenderer.sprite = activeCrop.preview;
                phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
                ps.Stop();
            }

            // Contextual menu switching
            if (mode != bc.mode)
            {
                if (bc.mode == "Build")
                {
                    SwapTo("Build");
                }
                else if (bc.mode == "Plant")
                {
                    SwapTo("Plant");
                }
                else if (bc.mode == "Unplaceable")
                {
                    SwapTo("Unplaceable");
                } else if (bc.mode == "StatShop")
                {
                    SwapTo("StatShop");
                }
            }
        }
    }

    // Enter build Mode
    public void BuildMode_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pc.state != PlayerControler.State.Downed && pc.state != PlayerControler.State.Rolling)
            {
                if (buildmodeActive)
                {
                    //Debug.Log("CLOSING BUILD MODE FROM: " + this.gameObject.name);
                    bc.inUpgrades = false;
                    buildmodeActive = false;
                    if (placeableHighlight != null)
                    {
                        placeableHighlight.SetActive(false);
                        if (popUp != null)
                            popUp.SetActive(false);
                        if (statShop != null)
                            statShop.SetActive(false);
                    }
                }
                else if (!buildmodeActive)
                {
                    //Debug.Log("OPENING BUILD MODE FROM: " + this.gameObject.name);
                    buildmodeActive = true;
                    if (placeableHighlight != null)
                    {
                        if (mode == "Build")
                        {
                            placeableHighlight.SetActive(true);
                            if (popUp != null)
                                popUp.SetActive(true);
                            // Set the higlight to match the prefab
                            pHSpriteRenderer.sprite = activePlaceable.preview;
                            phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                            popUpMan.switchMode("Build", count);
                            // Set hitbox to match the prefab
                            //bc.boxCol.size = activePlaceable.prefab.GetComponent<BoxCollider>().size;
                        }
                        else if (mode == "Plant")
                        {
                            placeableHighlight.SetActive(true);
                            if (popUp != null)
                                popUp.SetActive(true);
                            // Set the higlight to match the prefab
                            pHSpriteRenderer.sprite = activeCrop.preview;
                            phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
                            popUpMan.switchMode("Plant", count);
                            // Set hitbox to match the prefab
                            //bc.boxCol.size = activePlaceable.prefab.GetComponent<BoxCollider>().size;
                        } else if (mode == "StatShop")
                        {
                            placeableHighlight.SetActive(true);
                            ps.Stop();
                            if (statShop != null)
                                statShop.SetActive(true);
                        }

                        bc.mode = mode;
                        activePlaceable = popupUI.buildables[count];
                        activeCrop = popupUI.plantables[count];
                    }
                }
            }
        }
    }

    public void CloseBuildMode()
    {
        if (buildmodeActive)
        {
            buildmodeActive = false;
            if (placeableHighlight != null)
            {
                placeableHighlight.SetActive(false);
                if (popUp != null)
                    popUp.SetActive(false);
            }
        }
    }

    // Rotate Placeable 90 degrees
    public void Rotate_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Rotate currently disabled. As it is not needed");
            /*if (buildmodeActive)
            {
                if (placeableHighlight != null)
                {
                    Quaternion rotationAmmount = Quaternion.Euler(0, 90, 0);
                    placeableHighlight.transform.rotation *= rotationAmmount;
                }
            }*/
        }
    }

    // Instantiate Placeable
    public void Place_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                if (mode == "StatShop")
                {
                    if (grm.getMoney() >= upgradeCost)
                    {
                        Debug.Log("shop purchase");
                        psUpgrade.Emit(8);
                        grm.addMoney(-upgradeCost);
                        // individual upgrades probably should be migrated elsewhere
                        switch (count)
                        {
                            case 0:
                                EPD.playerHealth += healthUpgrade;
                                EPD.reviveHealth += healthUpgrade;
                                break;
                            case 1:
                                pc.damageBoost += damageUpgrade;

                                break;
                            case 2:
                                pc.moveSpeed += speedUpgrade;
                                pc.maxMoveSpeed += speedUpgrade;
                                pc.rollSpeedMax += speedUpgrade;
                                break;
                            case 3:
                                this.GetComponent<PlayerResourceManager>().maxCrops += carryUpgrade;

                                break;
                        }
                    }
                }

                if (placeableHighlight != null && acceptablePos)
                {
                    if (mode == "Build")
                    {
                        // Make sure the player can afford a turret before placing it. If they can, they pay the cost
                        var cost = activePlaceable.cost;
                        if (grm.getMoney() >= cost)
                        {
                            psDust.Emit(6);
                            grm.addMoney(-cost);
                            gameObject.GetComponent<AudioPlayer>().PlaySound(buildingSound);
                            animator.SetTrigger("Plant");
                            Instantiate(activePlaceable.prefab, placeableHighlight.transform.position, placeableHighlight.transform.rotation);
                        } else
                        {
                            Debug.Log("Nice try broke ass");
                        }
                    }
                    else if (mode == "Plant" && sm.state == SpawnManager.State.Break)
                    {
                        psDust.Emit(6);
                        animator.SetTrigger("Plant");
                        gameObject.GetComponent<AudioPlayer>().PlaySound(plantingSound);
                        GameObject newSpawner = Instantiate(activeCrop.prefab.gameObject, placeableHighlight.transform.position, placeableHighlight.transform.rotation);
                        sm.AddSpawner(newSpawner);
                        // Add bounty points
                        switch (count)
                        {
                            case 0:
                                grm.numCrop0Planted++;
                                grm.addBountyPoints(activeCrop, 0);
                                break;
                            case 1:
                                grm.numCrop1Planted++;
                                grm.addBountyPoints(activeCrop, 1);
                                break;
                            case 2:
                                grm.numCrop2Planted++;
                                grm.addBountyPoints(activeCrop, 2);
                                break;
                            case 3:
                                grm.numCrop3Planted++;
                                grm.addBountyPoints(activeCrop, 3);
                                break;
                        }
                    } 
                }
            }
        }
    }

    // Cycle Left to next buildable
    public void SwitchLeft_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive && mode != "Unplaceable")
            {
                if (count <= 0)
                    count = 3;
                else
                    count--;

                activePlaceable = popupUI.buildables[count];
                activeCrop = popupUI.plantables[count];
                AdjustRadius();

                switch (count)
                {
                    case 0:
                        popUpMan.selectTop();
                        statShopMan.selectTop();
                        break;
                    case 1:
                        popUpMan.selectRight();
                        statShopMan.selectRight();
                        break;
                    case 2:
                        popUpMan.selectBot();
                        statShopMan.selectBot();
                        break;
                    case 3:
                        popUpMan.selectLeft();
                        statShopMan.selectLeft();
                        break;
                }
            }
        }
    }

    // Cycle Right to next buildable
    public void SwitchRight_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive && mode != "Unplaceable")
            {
                if (count >= 3)
                    count = 0;
                else
                    count++;

                activePlaceable = popupUI.buildables[count];
                activeCrop = popupUI.plantables[count];
                AdjustRadius();

                switch (count)
                {
                    case 0:
                        popUpMan.selectTop();
                        statShopMan.selectTop();
                        break;
                    case 1:
                        popUpMan.selectRight();
                        statShopMan.selectRight();
                        break;
                    case 2:
                        popUpMan.selectBot();
                        statShopMan.selectBot();
                        break;
                    case 3:
                        popUpMan.selectLeft();
                        statShopMan.selectLeft();
                        break;
                }
            }
        }
    }

    // The following 4 functions are for directly selecting a placeable.
    // This selects the North placeable
    public void SelectNorth_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive && mode != "Unplaceable")
            {
                count = 0;
                popUpMan.selectTop();
                activePlaceable = popupUI.buildables[0];
                activeCrop = popupUI.plantables[0];
                AdjustRadius();

                statShopMan.selectTop();
            }
        }
    }

    // This selects the South placeable
    public void SelectSouth_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive && mode != "Unplaceable")
            {
                count = 2;
                popUpMan.selectBot();
                activePlaceable = popupUI.buildables[2];
                activeCrop = popupUI.plantables[2];
                AdjustRadius();

                statShopMan.selectBot();
            }
        }
    }

    // This selects the East placeable
    public void SelectEast_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive && mode != "Unplaceable")
            {
                count = 1;
                popUpMan.selectRight();
                activePlaceable = popupUI.buildables[1];
                activeCrop = popupUI.plantables[1];
                AdjustRadius();

                statShopMan.selectRight();
            }
        }
    }

    // This selects the West placeable
    public void SelectWest_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive && mode != "Unplaceable")
            {
                count = 3;
                popUpMan.selectLeft();
                activePlaceable = popupUI.buildables[3];
                activeCrop = popupUI.plantables[3];
                AdjustRadius();

                statShopMan.selectLeft();
            }
        }
    }

    // Swap from plants to buildables or vice versa NOW UNUSED DUE TO IMPLEMENTATION OF CONTEXTUAL MENU
    public void Swap_performed(InputAction.CallbackContext context)
{
    if (context.performed)
    {
        if (buildmodeActive)
        {
            /*if(pHSpriteRenderer == null)
            {
                phSprite = placeableHighlight.transform.GetChild(0).gameObject;
                pHSpriteRenderer = phSprite.GetComponent<SpriteRenderer>();
            }*/

            if (mode == "Build")
            {
                mode = "Plant";
                // Set the higlight to match the prefab
                pHSpriteRenderer.sprite = activeCrop.preview;
                phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
                ps.Stop();
                bc.mode = mode;
                activePlaceable = popupUI.buildables[0];
                activeCrop = popupUI.plantables[0];
                count = 0;
                if (popUp != null)
                    popUpMan.switchMode(mode, count);
            }
            else if (mode == "Plant")
            {
                mode = "Build";
                // Set the higlight to match the prefab
                pHSpriteRenderer.sprite = activePlaceable.preview;
                phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                ps.Play();
                bc.mode = mode;
                activePlaceable = popupUI.buildables[0];
                activeCrop = popupUI.plantables[0];
                count = 0;
                if (popUp != null)
                    popUpMan.switchMode(mode, count);
            }
            else
                Debug.LogError("Mode isn't Build or Plant");
        }
    }
}

    // Switches build menu to desired mode
    public void SwapTo(string toMode)
    {
        if (buildmodeActive)
        {
            mode = toMode;

            if (mode == "Build")
            {
                statShop.SetActive(false);
                popUp.SetActive(true);
                pHSpriteRenderer.enabled = true;
                pHSpriteRenderer.sprite = activePlaceable.preview;
                phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                AdjustRadius();
                ps.Play();
            }
            else if (mode == "Plant")
            {
                statShop.SetActive(false);
                popUp.SetActive(true);
                pHSpriteRenderer.enabled = true;
                pHSpriteRenderer.sprite = activeCrop.preview;
                phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
                ps.Stop();
            }
            else if (mode == "Unplaceable")
            {
                //popUp.SetActive(false);
                statShop.SetActive(false);
                //pHSpriteRenderer.enabled = false;
                ps.Stop();
            } else if (mode == "StatShop")
            {
                popUp.SetActive(false);
                statShop.SetActive(true);
                ps.Stop();
                pHSpriteRenderer.enabled = false;
            }

            //bc.mode = mode;
            activePlaceable = popupUI.buildables[count];
            activeCrop = popupUI.plantables[count];
            //count = 0;
            if (popUp != null)
                popUpMan.switchMode(mode, count);
        }
    }

    private void AlignToGrid(Transform trans)
    {
        // Get and round player position
        Vector3 playerPos = new Vector3(transform.position.x, 1, transform.position.z);
        xPos = Mathf.Round(playerPos.x);
        zPos = Mathf.Round(playerPos.z);
        xPos -= (xPos % gridSize);
        zPos -= (zPos % gridSize);

        // If aiming get aim direction
        /*if (pc.useControler)
        {
            // If aim is null, remeber last aimed place
            Debug.Log(pc.getDirection());
            if (pc.getDirection() != new Vector3(0, 0, 0))
            {
                xPos += (pc.getDirection().normalized.x);
                zPos += (pc.getDirection().normalized.z);
                prevDir = pc.getDirection();
            } else
            {
                xPos += (prevDir.normalized.x);
                zPos += (prevDir.normalized.z);
            }
        } else
        {
            xPos += (pc.getDirection().normalized.x);
            zPos += (pc.getDirection().normalized.z);
        }*/

        // align it to grid
        xPos -= (xPos % gridSize);
        zPos -= (zPos % gridSize);

        // Add offset
        xPos += gridOffsetX;
        zPos += gridOffsetZ;

        // Add to the X so its in front of the player
        if (pc.flipped)
            xPos -= (gridSize * 2);
        if(!pc.flipped)
            xPos += (gridSize);

        // Create and return position
        Vector3 placePos = new Vector3(xPos, trans.position.y, zPos);

        trans.position = placePos;
    }

    // Adjust the radius of the particle system to match the turret's range
    private void AdjustRadius()
    {
        var shape = ps.shape;
        shape.radius = activePlaceable.prefab.GetComponent<SphereCollider>().radius;
    }

    private void TestPlacementAvailable()
    {
        acceptablePos = false;

        if (bc.acceptablePos && !bc.intersectingBuildable)
        {
            if (mode == "Plant" && sm.state == SpawnManager.State.Break)
            {
                acceptablePos = true;
            } else if (mode == "Build")
            {
                acceptablePos = true;
            }
        }

        if (acceptablePos)
        {
            pHSpriteRenderer.color = Color.green;
        } else
        {
            pHSpriteRenderer.color = Color.red;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (buildmodeActive)
        {
            if (other.gameObject.tag == "TileObjective")
            {
                bc.inUpgrades = true;
                bc.mode = "StatShop";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (buildmodeActive)
        {
            if (other.gameObject.tag == "TileObjective")
            {
                bc.inUpgrades = false;
                bc.mode = "Unplaceable";
            }
        }
    }
}

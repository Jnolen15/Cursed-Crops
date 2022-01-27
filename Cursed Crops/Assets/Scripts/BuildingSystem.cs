using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    // Private variables
    [SerializeField] private popUpUISO popupUI;
    public GameObject popUp;
    private PlantingUIManager popUpMan;
    public PlaceableSO activePlaceable;
    public CropSO activeCrop;
    private PlayerControler pc;
    private BuildChecker bc;
    private SpawnManager sm;
    public GameObject placeableHighlight;
    public GameObject phSprite;
    public SpriteRenderer pHSpriteRenderer;
    private Animator animator;
    public bool acceptablePos;
    private int count = 0;

    // Public variables
    public bool buildmodeActive = false;
    public string mode = "Build";
    public float gridSize = 1;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    private void Start()
    {
        //placeableHighlight = this.transform.GetChild(4).gameObject;
        //phSprite = placeableHighlight.transform.GetChild(0).gameObject;
        //pHSpriteRenderer = phSprite.GetComponent<SpriteRenderer>();
        pc = this.GetComponent<PlayerControler>();
        bc = placeableHighlight.GetComponent<BuildChecker>();
        sm = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        animator = this.transform.GetChild(2).GetChild(0).gameObject.GetComponent<Animator>();
        popUp = Instantiate(popupUI.Prefab.gameObject, transform.position, transform.rotation, transform);
        popUp.SetActive(false);
        popUpMan = popUp.GetComponent<PlantingUIManager>();
        // Set default selected placeable
        if (popupUI.buildables.Length > 0 && popupUI.buildables.Length > 0)
        {
            activePlaceable = popupUI.buildables[0];
            activeCrop = popupUI.plantables[0];
        } else
        {
            Debug.Log("Placeable and/or Plantable array has a size of 0");
        }
        buildmodeActive = false;
        mode = "Build";
        placeableHighlight.SetActive(false);
        Debug.Log(this.gameObject.name);
    }


    private void Update()
    {
        if (buildmodeActive)
        {
            AlignToGrid(placeableHighlight.transform);
            AlignToGrid(popUp.transform);
            TestPlacementAvailable();

            if (mode == "Build" && pHSpriteRenderer.sprite != activePlaceable.preview)
            {
                pHSpriteRenderer.sprite = activePlaceable.preview;
                phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
            } else if (mode == "Plant" && pHSpriteRenderer.sprite != activePlaceable.preview)
            {
                pHSpriteRenderer.sprite = activeCrop.preview;
                phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
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
                    Debug.Log("CLOSING BUILD MODE FROM: " + this.gameObject.name);
                    buildmodeActive = false;
                    if (placeableHighlight != null)
                    {
                        placeableHighlight.SetActive(false);
                        if (popUp != null)
                            popUp.SetActive(false);
                    }
                }
                else if (!buildmodeActive)
                {
                    Debug.Log("OPENING BUILD MODE FROM: " + this.gameObject.name);
                    buildmodeActive = true;
                    if (placeableHighlight != null)
                    {
                        placeableHighlight.SetActive(true);
                        if (popUp != null)
                            popUp.SetActive(true);

                        if (mode == "Build")
                        {
                            // Set the higlight to match the prefab
                            pHSpriteRenderer.sprite = activePlaceable.preview;
                            phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                            bc.mode = mode;
                            popUpMan.selectTop();
                            // Set hitbox to match the prefab
                            //bc.boxCol.size = activePlaceable.prefab.GetComponent<BoxCollider>().size;
                        }
                        else if (mode == "Plant")
                        {
                            // Set the higlight to match the prefab
                            pHSpriteRenderer.sprite = activeCrop.preview;
                            phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
                            bc.mode = mode;
                            popUpMan.selectTop();
                            // Set hitbox to match the prefab
                            //bc.boxCol.size = activePlaceable.prefab.GetComponent<BoxCollider>().size;
                        }
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

    // Instantiate Placeeable
    public void Place_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                if (placeableHighlight != null && acceptablePos)
                {
                    if (mode == "Build")
                    {
                        animator.SetTrigger("Plant");
                        Instantiate(activePlaceable.prefab, placeableHighlight.transform.position, placeableHighlight.transform.rotation);
                    }
                    else if (mode == "Plant" && sm.state == SpawnManager.State.Break)
                    {
                        animator.SetTrigger("Plant");
                        GameObject newSpawner = Instantiate(activeCrop.prefab.gameObject, placeableHighlight.transform.position, placeableHighlight.transform.rotation);
                        sm.AddSpawner(newSpawner);
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
            if (buildmodeActive)
            {
                if (count <= 0)
                    count = 3;
                else
                    count--;

                activePlaceable = popupUI.buildables[count];
                activeCrop = popupUI.plantables[count];

                switch (count)
                {
                    case 0:
                        popUpMan.selectTop();
                        break;
                    case 1:
                        popUpMan.selectRight();
                        break;
                    case 2:
                        popUpMan.selectBot();
                        break;
                    case 3:
                        popUpMan.selectLeft();
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
            if (buildmodeActive)
            {
                if (count >= 3)
                    count = 0;
                else
                    count++;

                activePlaceable = popupUI.buildables[count];
                activeCrop = popupUI.plantables[count];

                switch (count)
                {
                    case 0:
                        popUpMan.selectTop();
                        break;
                    case 1:
                        popUpMan.selectRight();
                        break;
                    case 2:
                        popUpMan.selectBot();
                        break;
                    case 3:
                        popUpMan.selectLeft();
                        break;
                }

            }
        }
    }

    // The following 4 functions are for directly selecting a placeable. This selects the north placeable
    public void SelectNorth_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                popUpMan.selectTop();
                activePlaceable = popupUI.buildables[2];
                activeCrop = popupUI.plantables[2];
            }
        }
    }
    // This selects the South placeable
    public void SelectSouth_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                popUpMan.selectBot();
                activePlaceable = popupUI.buildables[1];
                activeCrop = popupUI.plantables[1];
            }
        }
    }
    // This selects the East placeable
    public void SelectEast_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                popUpMan.selectRight();
                activePlaceable = popupUI.buildables[0];
                activeCrop = popupUI.plantables[0];
            }
        }
    }
    // This selects the West placeable
    public void SelectWest_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                popUpMan.selectLeft();
                activePlaceable = popupUI.buildables[3];
                activeCrop = popupUI.plantables[3];
            }
        }
    }

    // Swap from plants to buildables or vice versa
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
                bc.mode = mode;
                if (popUp != null)
                    popUpMan.switchMode(mode);
            }
            else if (mode == "Plant")
            {
                mode = "Build";
                // Set the higlight to match the prefab
                pHSpriteRenderer.sprite = activePlaceable.preview;
                phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                bc.mode = mode;
                if (popUp != null)
                    popUpMan.switchMode(mode);
            }
            else
                Debug.LogError("Mode isn't Build or Plant");
        }
    }
}

    private void AlignToGrid(Transform trans)
    {
        // Get player position
        Vector3 playerPos = new Vector3(transform.position.x, 1, transform.position.z);

        // align it to grid
        float xPos = Mathf.Round(playerPos.x);
        xPos -= (xPos % gridSize);
        float zPos = Mathf.Round(playerPos.z);
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
        Vector3 placePos = new Vector3(xPos, 1, zPos);

        trans.position = placePos;
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
}

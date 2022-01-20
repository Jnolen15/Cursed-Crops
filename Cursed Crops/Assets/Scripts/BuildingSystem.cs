using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingSystem : MonoBehaviour
{
    // Private variables
    [SerializeField] private PlaceableSO activePlaceable;
    [SerializeField] private CropSO activeCrop;
    private PlayerControler pc;
    private BuildChecker bc;
    private SpawnManager sm;
    private GameObject placeableHighlight;
    private GameObject phSprite;
    private SpriteRenderer pHSpriteRenderer;
    public bool acceptablePos;

    // Public variables
    public bool buildmodeActive = false;
    public string mode = "Build";
    public float gridSize = 2;
    public float gridOffsetX = 0.5f;
    public float gridOffsetZ = 0.5f;

    private void Awake()
    {
        placeableHighlight = this.transform.GetChild(4).gameObject;
        phSprite = placeableHighlight.transform.GetChild(0).gameObject;
        pHSpriteRenderer = phSprite.GetComponent<SpriteRenderer>();
        pc = this.GetComponent<PlayerControler>();
        bc = placeableHighlight.GetComponent<BuildChecker>();

        sm = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }


    private void Update()
    {
        if (buildmodeActive)
        {
            alignToGrid(placeableHighlight.transform);
            testPlacementAvailable();
        }
    }

    // Enter build Mode
    public void BuildMode_performed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (buildmodeActive)
            {
                buildmodeActive = false;
                if (placeableHighlight != null)
                {
                    placeableHighlight.SetActive(false);
                }
            }
            else
            {
                buildmodeActive = true;
                if (placeableHighlight != null)
                {
                    placeableHighlight.SetActive(true);

                    if (mode == "Build")
                    {
                        // Set the higlight to match the prefab
                        pHSpriteRenderer.sprite = activePlaceable.preview;
                        phSprite.transform.localScale = activePlaceable.prefab.GetChild(0).GetChild(0).transform.localScale;
                        bc.mode = mode;
                        // Set hitbox to match the prefab
                        //bc.boxCol.size = activePlaceable.prefab.GetComponent<BoxCollider>().size;
                    }
                    else if (mode == "Plant")
                    {
                        // Set the higlight to match the prefab
                        pHSpriteRenderer.sprite = activeCrop.preview;
                        phSprite.transform.localScale = activeCrop.prefab.GetChild(0).GetChild(0).transform.localScale;
                        bc.mode = mode;
                        // Set hitbox to match the prefab
                        //bc.boxCol.size = activePlaceable.prefab.GetComponent<BoxCollider>().size;
                    }
                }
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
                        Instantiate(activePlaceable.prefab, placeableHighlight.transform.position, placeableHighlight.transform.rotation);
                    else if (mode == "Plant" && sm.state == SpawnManager.State.Break)
                    {
                        GameObject newSpawner = Instantiate(activeCrop.prefab.gameObject, placeableHighlight.transform.position, placeableHighlight.transform.rotation);
                        sm.AddSpawner(newSpawner);
                    }
                }
            }
        }
    }

    private void alignToGrid(Transform trans)
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

    private void testPlacementAvailable()
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

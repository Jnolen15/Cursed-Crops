using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantingUIManager : MonoBehaviour
{

    // textBoxes
    public GameObject Window;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text stats;

    // SelectionUI
    public GameObject TopSelect;
    public GameObject BotSelect;
    public GameObject RightSelect;
    public GameObject LeftSelect;

    // external objects
    public Camera mainCamera;
    public popUpUISO SO;

    // private text strings
    private string[] A = new string[4];
    private string[] B = new string[4];
    private string[] C = new string[4];
    private string[] D = new string[4];




    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // SO setup was here but changed it to its own function called by Building System.
        // This way we only need 1 UI prefab instead of one for each level
    }

    public void InitializeSO(popUpUISO so)
    {
        SO = so;

        if (SO != null)
        {
            string[] textA = new string[] { SO.buildables[0].placeableName, SO.buildables[0].desc, SO.buildables[0].stats, SO.buildables[0].price };
            string[] textB = new string[] { SO.buildables[1].placeableName, SO.buildables[1].desc, SO.buildables[1].stats, SO.buildables[1].price };
            string[] textC = new string[] { SO.buildables[2].placeableName, SO.buildables[2].desc, SO.buildables[2].stats, SO.buildables[2].price };
            string[] textD = new string[] { SO.buildables[3].placeableName, SO.buildables[3].desc, SO.buildables[3].stats, SO.buildables[3].price };
            setUp(textA, textB, textC, textD);
            this.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("UI scriptable object ref is null");
        }
    }

    // method for setting up the box, takes the title, description, stats, and price strings for each textbox and stores them in their
    // respective arrays. Also sets the price boxes from the start because they are always displayed.
    // A is rightButton, B is bottom, C is top, and D is left, sort of like a switch controller
    public void setUp(string[] newA, string[] newB, string[] newC, string[] newD)
    {
        A = newA;
        B = newB;
        C = newC;
        D = newD;

        //Debug.Log("setUp");
    }

    public void switchMode(string mode, int count)
    {
        if (mode == "Build")
        {
            string[] textA = new string[] { SO.buildables[0].placeableName, SO.buildables[0].desc, SO.buildables[0].stats, SO.buildables[0].price };
            string[] textB = new string[] { SO.buildables[1].placeableName, SO.buildables[1].desc, SO.buildables[1].stats, SO.buildables[1].price };
            string[] textC = new string[] { SO.buildables[2].placeableName, SO.buildables[2].desc, SO.buildables[2].stats, SO.buildables[2].price };
            string[] textD = new string[] { SO.buildables[3].placeableName, SO.buildables[3].desc, SO.buildables[3].stats, SO.buildables[3].price };
            setUp(textA, textB, textC, textD);
            switch (count)
            {
                case 0:
                    selectTop();
                    break;
                case 1:
                    selectRight();
                    break;
                case 2:
                    selectBot();
                    break;
                case 3:
                    selectLeft();
                    break;
            }
        } else if (mode == "Plant")
        {
            string[] textA = new string[] { SO.plantables[0].cropName, SO.plantables[0].desc, SO.plantables[0].stats, SO.plantables[0].price };
            string[] textB = new string[] { SO.plantables[1].cropName, SO.plantables[1].desc, SO.plantables[1].stats, SO.plantables[1].price };
            string[] textC = new string[] { SO.plantables[2].cropName, SO.plantables[2].desc, SO.plantables[2].stats, SO.plantables[2].price };
            string[] textD = new string[] { SO.plantables[3].cropName, SO.plantables[3].desc, SO.plantables[3].stats, SO.plantables[3].price };
            setUp(textA, textB, textC, textD);
            switch (count)
            {
                case 0:
                    selectTop();
                    break;
                case 1:
                    selectRight();
                    break;
                case 2:
                    selectBot();
                    break;
                case 3:
                    selectLeft();
                    break;
            }
        }
        else if (mode == "Unplaceable")
        {
            closeDisplay();
        } else if (mode == "StatShop")
        {
            // closeDisplay();
        } else
        {
            Debug.LogError("switchMode was not given a propper mode: " + mode);
        }
    }

    // methods for displaying the textbox according to which window is selected
    public void selectRight()
    {
        closeDisplay();
        display(B[0], B[1], B[2]);
        RightSelect.SetActive(true);
    }
    public void selectBot()
    {
        closeDisplay();
        display(C[0], C[1], C[2]);
        BotSelect.SetActive(true);
    }
    public void selectTop()
    {
        closeDisplay();
        display(A[0], A[1], A[2]);
        TopSelect.SetActive(true);
    }
    public void selectLeft()
    {
        closeDisplay();
        display(D[0], D[1], D[2]);
        LeftSelect.SetActive(true);
    }


    private void display(string a, string b, string c)
    {
        Window.SetActive(true);
        title.text = a;
        description.text = b;
        stats.text = c;
    }

    public void EditCost(bool isCrop, int cost)
    {
        if (isCrop)
        {
            stats.text = "Quota value: " + cost;
        } else
        {
            stats.text = "Cost: " + cost;
        }
    }

    // closes the window display (which is right of the buttons)
    public void closeDisplay()
    {
        Window.SetActive(false);
        TopSelect.SetActive(false);
        BotSelect.SetActive(false);
        RightSelect.SetActive(false);
        LeftSelect.SetActive(false);
    }

    // deletes the display entirely
    public void exit()
    {
        Destroy(this.gameObject);
    }

}

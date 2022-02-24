using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatShopUIManager : MonoBehaviour
{

    // textBoxes
    public TMP_Text topNum;
    public TMP_Text botNum;
    public TMP_Text rightNum;
    public TMP_Text leftNum;


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
    public statShopSO SO;

    // private text strings
    private string[] A = new string[4];
    private string[] B = new string[4];
    private string[] C = new string[4];
    private string[] D = new string[4];




    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (SO != null)
        {
            string[] textA = SO.textA;
            string[] textB = SO.textB;
            string[] textC = SO.textC;
            string[] textD = SO.textD;
            setUp(textA, textB, textC, textD);
        } else
        {
            Debug.LogError("UI scriptable object ref is null");
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        /*// testing internal functions
        closeDisplay();
        if (Input.GetKey(KeyCode.I))
        {
            selectTop();
        } 
        if (Input.GetKey(KeyCode.K))
        {
            selectBot();
        } 
        if (Input.GetKey(KeyCode.L))
        {
            selectRight();
        } 
        if (Input.GetKey(KeyCode.J))
        {
            selectLeft();
        }*/
        

        // pointing towards camera (but only x-axis rotation)
        /*this.transform.LookAt(mainCamera.transform, Vector3.up);
        this.transform.rotation *= Quaternion.Euler(0, 180, 0);
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, 0, 0);*/
        // why is this two statements instead of one? I tried it, it broke. tldr: I have no clue how quarternions work
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


        rightNum.text = A[3];
        botNum.text = B[3];
        topNum.text = C[3];
        leftNum.text = D[3];

        Debug.Log("setUp");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantingUIManager : MonoBehaviour
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

    public Camera mainCamera;

    // private text strings
    private string[] A = new string[4];
    private string[] B = new string[4];
    private string[] C = new string[4];
    private string[] D = new string[4];




    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindObjectOfType<Camera>();
        setUp("a", "a", "a", "a", "b", "b", "b", "b", "c", "c", "c", "c", "d", "d", "d", "d");
    }

    // Update is called once per frame
    void Update()
    {
        // testing internal functions
        closeDisplay();
        if (Input.GetKey(KeyCode.W))
        {
            selectTop();
        } 
        if (Input.GetKey(KeyCode.S))
        {
            selectBot();
        } 
        if (Input.GetKey(KeyCode.D))
        {
            selectRight();
            Debug.Log("displayRight");
        } 
        if (Input.GetKey(KeyCode.A))
        {
            selectLeft();
        }

        // pointing towards camera (but only x-axis rotation)
        this.transform.LookAt(mainCamera.transform, Vector3.up);
        this.transform.rotation *= Quaternion.Euler(0, 180, 0);
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, 0, 0);
        // why is this two statements instead of one? I tried it, it broke. tldr: I have no clue how quarternions work
    }

    // method for setting up the box, takes the title, description, stats, and price strings for each textbox and stores them in their
    // respective arrays. Also sets the price boxes from the start because they are always displayed.
    // A is rightButton, B is bottom, C is top, and D is left, sort of like a switch controller
    public void setUp(string tA, string dA, string sA, string pA, string tB, string dB, string sB, string pB,
        string tC, string dC, string sC, string pC, string tD, string dD, string sD, string pD)
    {
        A[0] = tA;
        A[1] = dA;
        A[2] = sA;
        A[3] = pA;

        B[0] = tB;
        B[1] = dB;
        B[2] = sB;
        B[3] = pB;

        C[0] = tC;
        C[1] = dC;
        C[2] = sC;
        C[3] = pD;

        D[0] = tD;
        D[1] = dD;
        D[2] = sD;
        D[3] = pD;


        rightNum.text = A[3];
        botNum.text = B[3];
        topNum.text = C[3];
        leftNum.text = D[3];

        Debug.Log("setUp");
    }

    // methods for displaying the textbox according to which window is selected
    public void selectRight()
    {
        display(A[0], A[1], A[2]);
        RightSelect.SetActive(true);
    }
    public void selectBot()
    {
        display(B[0], B[1], B[2]);
        BotSelect.SetActive(true);
    }
    public void selectTop()
    {
        display(C[0], C[1], C[2]);
        TopSelect.SetActive(true);
    }
    public void selectLeft()
    {
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

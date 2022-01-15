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






    // Start is called before the first frame update
    void Start()
    {
        topNum.text = "100";
        botNum.text = "100";
        rightNum.text = "100";
        leftNum.text = "100";

        mainCamera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // testing internal functions
        string a;
        closeDisplay();
        if (Input.GetKey(KeyCode.W))
        {
            a = "top";
            selectTop(a, a, a);
        } 
        if (Input.GetKey(KeyCode.S))
        {
            a = "bot";
            selectBot(a, a, a);
        } 
        if (Input.GetKey(KeyCode.D))
        {
            a = "right";
            selectRight(a, a, a);
        } 
        if (Input.GetKey(KeyCode.A))
        {
            a = "left";
            selectLeft(a, a, a);
        }

        // pointing towards camera
            
    }


    public void selectTop(string a, string b, string c)
    {
        display(a, b, c);
        TopSelect.SetActive(true);
    }
    public void selectBot(string a, string b, string c)
    {
        display(a, b, c);
        BotSelect.SetActive(true);
    }
    public void selectRight(string a, string b, string c)
    {
        display(a, b, c);
        RightSelect.SetActive(true);
    }
    public void selectLeft(string a, string b, string c)
    {
        display(a, b, c);
        LeftSelect.SetActive(true);
    }


    public void display(string a, string b, string c)
    {
        Window.SetActive(true);
        title.text = a;
        description.text = b;
        stats.text = c;
    }

    public void closeDisplay()
    {
        Window.SetActive(false);
        TopSelect.SetActive(false);
        BotSelect.SetActive(false);
        RightSelect.SetActive(false);
        LeftSelect.SetActive(false);
    }

    public void exit()
    {
        Destroy(this.gameObject);
    }

}

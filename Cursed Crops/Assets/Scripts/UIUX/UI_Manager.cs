using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    private PlayerControler PC;
    public GameObject p1UI;
    // UI elements must me dragged/droppped
    public TextMeshProUGUI currencyText;
    public Slider HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        // aquiring player controller
        // dependant on player name, might need to be improved
        // PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>();
    } 

    // Update is called once per frame
    void Update()
    {
        if (PC == null && GameObject.Find("Player(Clone)") != null)
        {
            PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>();
            // also need to catch the error created by not finding a player initially
            p1UI.SetActive(true);
        }

        // checking if PC has been aquired
        if (PC != null)
        {
            // managing  currency text
            currencyText.text = "Monies: " + PC.money;
            // managing health bar
            HealthBar.value = PC.Health / PC.MaxHealth;
        }
    }

    
}

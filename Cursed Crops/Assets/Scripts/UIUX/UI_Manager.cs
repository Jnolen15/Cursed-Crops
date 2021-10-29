using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    private PlayerControler PC;
    // UI elements must me dragged/droppped
    public TextMeshProUGUI currencyText;
    public Slider HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        // aquiring player controller
        // dependant on player name, might need to be improved
        PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>(); // will be null if no players have dropped in
    } 

    // Update is called once per frame
    void Update()
    {
        if (PC == null)
        {
            PC = GameObject.Find("Player(Clone)").GetComponent<PlayerControler>();
            // also need to catch the error created by not finding a player initially
        }

        // managing currency text
        if (PC == null)
        {
            currencyText.text = "Monies: 0"; 
        } else
        {
            currencyText.text = "Monies: " + PC.money;
        }

        // managing health bar
        HealthBar.value = PC.Health/PC.MaxHealth;
    }

    
}

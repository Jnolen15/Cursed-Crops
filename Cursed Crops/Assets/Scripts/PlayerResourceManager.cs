using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    public int cropsValue = 5;

    private int crops = 0;
    private GameRuleManager grm;

    public void setCrops(int newAmount) { crops = newAmount; }
    public int getCrops() { return crops; }
    public void addCrops(int amount) { crops += amount; }

    public void Start()
    {
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
    }

    public void BankItems()
    {
        //addMoney(crops * cropsValue);
        grm.addMoney(crops * cropsValue);
        setCrops(0);
    }

    public void UpdateUI()
    {

    }

    // --- DEBUG Methods
    public void PrintCurrentInventory()
    {
        Debug.Log("Current Inventory --- \nmoney: " + grm.getMoney() + "\n crops: " + crops + "-------------");
    }
}

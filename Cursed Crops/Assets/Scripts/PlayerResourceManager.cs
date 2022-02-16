using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    public float cropsValue = 5f;

    private float money = 0f;
    private int crops = 0;

    public void setMoney(float newAmount) { money = newAmount; }
    public void setCrops(int newAmount) { crops = newAmount; }
    public float getMoney() { return money; }
    public int getCrops() { return crops; }
    public void addMoney(float amount) { money += amount; }
    public void addCrops(int amount) { crops += amount; }

    public void BankItems()
    {
        addMoney(crops * cropsValue);
        setCrops(0);
    }

    public void UpdateUI()
    {

    }

    // --- DEBUG Methods
    public void PrintCurrentInventory()
    {
        Debug.Log("Current Inventory --- \nmoney: " + money + "\n crops: " + crops);
    }
}

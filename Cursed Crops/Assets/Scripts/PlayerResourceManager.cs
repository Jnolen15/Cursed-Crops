using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    public float redCropsValue = 5f;
    public float purpleCropsValue = 10f;

    private float money = 0f;
    private int redCrops = 0;
    private int purpleCrops = 0;

    public void setMoney(float newAmount) { money = newAmount; }
    public void setRedCrops(int newAmount) { redCrops = newAmount; }
    public void setPurpleCrops(int newAmount) { purpleCrops = newAmount; }

    public float getMoney() { return money; }
    public int getRedCrops() { return redCrops; }
    public int getPurpleCrops() { return purpleCrops; }

    public void addMoney(float amount) { money += amount; }
    public void addRedCrops(int amount) { redCrops += amount; }
    public void addPurpleCrops(int amount) { purpleCrops += amount; }

    public void addDroppedItem(int amount, ItemDrop.DropType type)
    {
        if (type == ItemDrop.DropType.red)
        {
            addRedCrops(amount);
        }
        else if (type == ItemDrop.DropType.purple)
        {
            addPurpleCrops(amount);
        }
        else
        {
            Debug.Log("ERROR - Need to select an appropriate DropType for ItemDrop");
        }
    }

    public void BankItems()
    {
        addMoney(redCrops * redCropsValue);
        setRedCrops(0);
        addMoney(purpleCrops * purpleCropsValue);
        setPurpleCrops(0);
    }

    public void UpdateUI()
    {

    }

    // --- DEBUG Methods
    public void PrintCurrentInventory()
    {
        Debug.Log("Current Inventory --- \nmoney: " + money + "\n red crops: " + redCrops + "\npurple crops: " + purpleCrops);
    }
}

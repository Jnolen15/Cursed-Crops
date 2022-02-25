using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    public int cropsValue = 5;
    public int maxCrops = 10;
    private int crops = 0;
    private GameRuleManager grm;

    public ParticleSystem ps;
    private GameObject pickupPS;

    public void setCrops(int newAmount) { crops = newAmount; }
    public int getCrops() { return crops; }
    public void addCrops(int amount) { crops += amount; }

    public void Start()
    {
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();

        pickupPS = Resources.Load<GameObject>("Effects/PickupParticle");
        ps = Instantiate(pickupPS, transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        ps.Pause();
    }

    public void BankItems()
    {
        //addMoney(crops * cropsValue);
        grm.addMoney(crops * cropsValue);
        setCrops(0);
    }

    public void UpdateUI()
    {
        // IDK why this is here. Delete if remains unsused
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DroppedItem" && crops < maxCrops)
        {
            ItemDrop itemDrop = other.GetComponent<ItemDrop>();
            var itemDiff = maxCrops - crops;
            // If pickup contains more than can cary take only some
            if (itemDrop.value > itemDiff)
            {
                itemDrop.ReduceValue(itemDiff);
                addCrops(itemDiff);
            }
            // Take all
            else
            {
                addCrops(itemDrop.value);
                itemDrop.GetPickedUp();
            }

            if (ps != null)
                ps.Emit(6);
        }
    }

    // --- DEBUG Methods
    public void PrintCurrentInventory()
    {
        Debug.Log("Current Inventory --- \nmoney: " + grm.getMoney() + "\n crops: " + crops + "-------------");
    }
}

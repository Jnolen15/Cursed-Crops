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
    private GameObject itemSprite;

    public void setCrops(int newAmount) { crops = newAmount; }
    public int getCrops() { return crops; }
    public void addCrops(int amount) { crops += amount; }

    public void Start()
    {
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();

        itemSprite = Resources.Load<GameObject>("Effects/ItemSprite");
        pickupPS = Resources.Load<GameObject>("Effects/PickupParticle");
        ps = Instantiate(pickupPS, transform.position, transform.rotation, transform).GetComponent<ParticleSystem>();
        ps.Pause();
    }

    public void BankItems()
    {
        grm.addMoney(crops * cropsValue);
        setCrops(0);
    }

    IEnumerator MoveItems(int numCrops, Transform objective)
    {
        for (int i = 0; i < numCrops; i++)
        {
            float time = 0;
            float duration = 0.1f;
            float durOffset;
            //if ((numCrops * 0.01f) > duration) durOffset = numCrops * 0.01f;
            //else durOffset = 0.19f;
            //duration = (0.2f - durOffset);
            GameObject item = Instantiate(itemSprite, this.transform.position, this.transform.rotation);
            while (time < duration)
            {
                item.transform.position = Vector3.Lerp(this.transform.position, objective.position, time / duration);

                time += Time.deltaTime;
                yield return null;
            }
            Destroy(item);
        }
    }

    public void UpdateUI()
    {
        // IDK why this is here. Delete if remains unsused
    }

    private void OnTriggerEnter(Collider other)
    {
        // if colliding with an item pick it up
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

        // if colliding with the farm house, bank items
        if (other.gameObject.name == "InteractionDistance")
        {
            StartCoroutine(MoveItems(getCrops(), other.transform));
            BankItems();
        }
    }

    // --- DEBUG Methods
    public void PrintCurrentInventory()
    {
        Debug.Log("Current Inventory --- \nmoney: " + grm.getMoney() + "\n crops: " + crops + "-------------");
    }
}

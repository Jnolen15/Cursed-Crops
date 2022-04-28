using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    // ======== Private variables ========
    private PlayerManager pm;
    private GameRuleManager grm;
    private GameObject psUpgrade;

    private int upgradeCostIncremet = 50;
    private int healthUpgrade = 3;
    private float damageUpgrade = 0.5f;
    private float speedUpgrade = 1;
    private int carryUpgrade = 10;

    // ======== Public variables ========
    public int healthUpgradeCost = 100;
    public int damageUpgradeCost = 100;
    public int speedUpgradeCost = 100;
    public int carryUpgradeCost = 100;

    public float healthUpgradeTier = 1;
    public float damageUpgradeTier = 1;
    public float speedUpgradeTier = 1;
    public float carryUpgradeTier = 1;

    /* WRITEUP ON HOW NEW SYSTEM WORKS
     * In building system in Place_performed now all it does is call BuyUpgrade for the selected upgrade.
     * Then BuyUpgrade checks if the players can afford it. If so it calls ApplyUpgrade and UpdateShopUI.
     * It also subtracts the cost, and increments that upgrades price and teir.
     * ApplyUpgrade Loops through the list of players and applies the upgrade effects
     * UpdateShopUI Updates the shop UI text to reflect the price and tier changes.
     * For now I just manually change the string arrays used in the StatShopUIManager.
     * Then I call a new function in StatShopUIManager to refresh the display.
     * 
     * TO DO:
     * Refreshing the display will update the display for all players.
     * So if two players are in the menu and one has damage slected but the other buys health
     * their UI will also change to health, even though they still have damage selected.
     */

    void Start()
    {
        pm = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
        psUpgrade = Resources.Load<GameObject>("Effects/UpgradeParticle");
    }

    public void BuyUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "Health":
                if (grm.getMoney() >= healthUpgradeCost)
                {
                    grm.addMoney(-healthUpgradeCost);
                    healthUpgradeTier++;
                    healthUpgradeCost += upgradeCostIncremet;
                    ApplyUpgrade("Health");
                    UpdateShopUI("Health");
                }
                break;
            case "Damage":
                if (grm.getMoney() >= damageUpgradeCost)
                {
                    grm.addMoney(-damageUpgradeCost);
                    damageUpgradeTier++;
                    damageUpgradeCost += upgradeCostIncremet;
                    ApplyUpgrade("Damage");
                    UpdateShopUI("Damage");
                }
                break;
            case "Speed":
                if (grm.getMoney() >= speedUpgradeCost)
                {
                    grm.addMoney(-speedUpgradeCost);
                    speedUpgradeTier++;
                    speedUpgradeCost += upgradeCostIncremet;
                    ApplyUpgrade("Speed");
                    UpdateShopUI("Speed");
                }
                break;
            case "Carry":
                if (grm.getMoney() >= carryUpgradeCost)
                {
                    grm.addMoney(-carryUpgradeCost);
                    carryUpgradeTier++;
                    carryUpgradeCost += upgradeCostIncremet;
                    ApplyUpgrade("Carry");
                    UpdateShopUI("Carry");
                }
                break;
        }
    }

    private void ApplyUpgrade(string upgrade)
    {
        Debug.Log("Applying " + upgrade + " to all players.");

        for (int i = 0; i < pm.players.Count; i++)
        {
            GameObject player = pm.players[i];

            switch (upgrade)
            {
                case "Health":
                    player.GetComponent<EnemyPlayerDamage>().playerHealth += healthUpgrade;
                    player.GetComponent<EnemyPlayerDamage>().reviveHealth += healthUpgrade;
                    break;
                case "Damage":
                    player.GetComponent<PlayerControler>().damageBoost += damageUpgrade;
                    break;
                case "Speed":
                    player.GetComponent<PlayerControler>().moveSpeed += speedUpgrade;
                    player.GetComponent<PlayerControler>().maxMoveSpeed += speedUpgrade;
                    player.GetComponent<PlayerControler>().rollSpeedMax += speedUpgrade;
                    break;
                case "Carry":
                    player.GetComponent<PlayerResourceManager>().maxCrops += carryUpgrade;
                    break;
            }

            Instantiate(psUpgrade, player.transform.position, player.transform.rotation);
        }
    }

    private void UpdateShopUI(string upgrade)
    {
        for (int i = 0; i < pm.players.Count; i++)
        {
            GameObject player = pm.players[i];
            BuildingSystem bs = player.GetComponent<BuildingSystem>();
            StatShopUIManager shopUI = bs.statShop.GetComponent<StatShopUIManager>();

            switch (upgrade)
            {
                case "Health":
                    shopUI.A[0] = "Health " + healthUpgradeTier;
                    shopUI.A[2] = "Cost: " + healthUpgradeCost;
                    shopUI.refreshDisplay('A');
                    break;
                case "Damage":
                    shopUI.B[0] = "Damage " + damageUpgradeTier;
                    shopUI.B[2] = "Cost: " + damageUpgradeCost;
                    shopUI.refreshDisplay('B');
                    break;
                case "Speed":
                    shopUI.C[0] = "Speed " + speedUpgradeTier;
                    shopUI.C[2] = "Cost: " + speedUpgradeCost;
                    shopUI.refreshDisplay('C');
                    break;
                case "Carry":
                    shopUI.D[0] = "Carry Capacity " + carryUpgradeTier;
                    shopUI.D[2] = "Cost: " + carryUpgradeCost;
                    shopUI.refreshDisplay('D');
                    break;
            }
        }
    }
}

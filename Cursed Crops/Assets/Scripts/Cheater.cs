using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cheater : MonoBehaviour
{
    PlayerManager pm;
    GameRuleManager grm;
    PlayerResourceManager prm;
    PlayerControler pc;
    EnemyPlayerDamage epd;
    SpawnManager sm;
    EnemyDamageObjective edo;

    private int devMode;

    void Start()
    {
        pm = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
        sm = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        edo = GameObject.Find("Objective").GetComponent<EnemyDamageObjective>();
    }

    void Update()
    {
        if (pm.players.Count > 0)
        {
            prm = pm.players[0].gameObject.GetComponent<PlayerResourceManager>();
            pc = pm.players[0].gameObject.GetComponent<PlayerControler>();
            epd = pm.players[0].gameObject.GetComponent<EnemyPlayerDamage>();
        }

        //Cheat codes. All cheat codes that apply to players only apply to player 1
        devMode = PlayerPrefs.GetInt("DevMode");
        if (devMode == 1)
        {
            // Add 100$ to players money total
            if (Keyboard.current[Key.M].wasPressedThisFrame)
                grm.addMoney(100);

            // Add 100 points to the player's quota requirement
            if (Keyboard.current[Key.N].wasPressedThisFrame)
                grm.addPoints(100);

            // Max heal the player
            if (Keyboard.current[Key.H].wasPressedThisFrame)
                epd.Heal(pc.maxHealth);

            // Damage the player by 2
            if (Keyboard.current[Key.J].wasPressedThisFrame)
                epd.Damage(2);

            // Progress the phase forward 30 seconds
            if (Keyboard.current[Key.V].wasPressedThisFrame)
                sm.elapsedTime += 30;

            // Add 10 Crops to the players inventory
            if (Keyboard.current[Key.B].wasPressedThisFrame)
                prm.addCrops(10);

            // The objective House health takes 50 damage
            if (Keyboard.current[Key.L].wasPressedThisFrame)
                edo.houseHealth -= 50;
        }
    }
}

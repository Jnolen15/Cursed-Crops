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

        //Cheat codes to add money, Increase the plant bounty, and heal the player
        if (Keyboard.current[Key.M].wasPressedThisFrame)
            grm.addMoney(100);

        if (Keyboard.current[Key.N].wasPressedThisFrame)
            grm.addPoints(100);

        if (Keyboard.current[Key.H].wasPressedThisFrame)
            epd.Heal(pc.maxHealth);

        if (Keyboard.current[Key.V].wasPressedThisFrame)
            sm.elapsedTime += 30;

        if (Keyboard.current[Key.B].wasPressedThisFrame)
            prm.addCrops(10);

        if (Keyboard.current[Key.L].wasPressedThisFrame)
            edo.houseHealth -= 50;
    }
}

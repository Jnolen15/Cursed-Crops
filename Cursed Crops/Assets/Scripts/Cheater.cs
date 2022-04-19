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

    void Start()
    {
        pm = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        grm = GameObject.Find("GameRuleManager").GetComponent<GameRuleManager>();
    }

    void Update()
    {
        if (pm.players.Count > 0)
        {
            prm = pm.players[0].gameObject.GetComponent<PlayerResourceManager>();
            pc = pm.players[0].gameObject.GetComponent<PlayerControler>();
            epd = pm.players[0].gameObject.GetComponent<EnemyPlayerDamage>();
        }

        if (Keyboard.current[Key.M].wasPressedThisFrame)
            grm.addMoney(100);

        if (Keyboard.current[Key.N].wasPressedThisFrame)
            grm.addPoints(100);

        if (Keyboard.current[Key.H].wasPressedThisFrame)
            epd.Heal(pc.maxHealth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        if (Input.GetKeyDown(KeyCode.M))
            grm.addMoney(100);

        if (Input.GetKeyDown(KeyCode.K))
            grm.addPoints(100);

        if (Input.GetKeyDown(KeyCode.H))
            epd.Heal(pc.maxHealth);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuleManager : MonoBehaviour
{
    // ================ Public ================
    public int startingMoney = 100;

    // ================ Private ================
    private int globalMoney = 0;

    public float getMoney() { return globalMoney; }
    public void addMoney(int amount) { globalMoney += amount; }

    // Start is called before the first frame update
    void Start()
    {
        globalMoney += startingMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject Shop;
    public void loadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void exitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void toggleShop()
    {
        Shop.SetActive(!Shop.activeSelf);
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject Shop;
    public void loadLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void exitToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void toggleShop()
    {
        Shop.SetActive(!Shop.activeSelf);
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject Shop;
    public void loadLevel()
    {
        //Will just comment it out for now since this will go to controls first
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("Controls");
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

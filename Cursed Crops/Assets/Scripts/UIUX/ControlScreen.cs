using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlScreen : MonoBehaviour
{
    //Load the level
    public void loadLevel()
    {
        SceneManager.LoadScene("Level1");
    }
}

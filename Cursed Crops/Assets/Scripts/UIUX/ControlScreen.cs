using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ControlScreen : MonoBehaviour, ICancelHandler
{
    public GameObject basicsSlide;
    public GameObject buildingSlide;
    public GameObject controllsSlide;
    public GameObject loadingSlide;
    public Button next;

    private int count = 0;

    public void Progress()
    {
        switch (count)
        {
            case 0:
                basicsSlide.SetActive(false);
                buildingSlide.SetActive(true);
                count++;
                break;
            case 1:
                buildingSlide.SetActive(false);
                controllsSlide.SetActive(true);
                count++;
                break;
            case 2:
                controllsSlide.SetActive(false);
                loadingSlide.SetActive(true);
                loadLevel();
                break;
        }
    }

    public void loadLevel()
    {
        //SceneManager.LoadScene("Level1");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnCancel(BaseEventData eventData)
    {
        Debug.Log("CANCEL CALLED");
    }

}

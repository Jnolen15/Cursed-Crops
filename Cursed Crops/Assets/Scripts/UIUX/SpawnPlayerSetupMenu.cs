using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class SpawnPlayerSetupMenu : MonoBehaviour
{
    //public GameObject playerSetypMenuPrefab;
    public PlayerInput input;
    public GameObject menu;

    private void Awake()
    {
        /*var rootMenu = GameObject.Find("MainLayout");
        if (rootMenu != null)
        {
            var menu = Instantiate(playerSetypMenuPrefab, rootMenu.transform);
            input.uiInputModule = menu.GetComponentInChildren<InputSystemUIInputModule>();
            menu.GetComponent<CharacterSelectWheel>().SetPlayerIndex(input.playerIndex);
        }*/

        menu = transform.Find("Canvas/Character Select Wheel").gameObject;
        input.uiInputModule = transform.Find("EventSystem").gameObject.GetComponentInChildren<InputSystemUIInputModule>();
        menu.GetComponent<CharacterSelectWheel>().SetPlayerIndex(input.playerIndex);

        // Adjust position
        if(input.playerIndex == 0)
            menu.GetComponent<RectTransform>().localPosition += new Vector3(-300, 0, 0);
        else if (input.playerIndex == 1)
            menu.GetComponent<RectTransform>().localPosition += new Vector3(-100, 0, 0);
        else if(input.playerIndex == 2)
            menu.GetComponent<RectTransform>().localPosition += new Vector3(100, 0, 0);
        else if(input.playerIndex == 3)
            menu.GetComponent<RectTransform>().localPosition += new Vector3(300, 0, 0);
    }
}

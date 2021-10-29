using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference roll = null;
    [SerializeField] public PlayerControler playerControler = null; // Possibly just change to PlayerInput
    [SerializeField] private TMP_Text bindingDisplayNameText = null;
    [SerializeField] private GameObject startRebindObject = null;
    [SerializeField] private GameObject waitingForInputObject = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void StartRebinding()
    {
        startRebindObject.SetActive(false);
        waitingForInputObject.SetActive(true);

        //Debug.Log(playerControler.PlayerInput.currentActionMap);
        //playerControler.PlayerInput.currentActionMap.Disable();
        //playerControler.PlayerInput.SwitchCurrentActionMap("Menu");
        playerControler.PlayerInput.actions.FindActionMap("Menu").Enable();
        playerControler.PlayerInput.actions.FindActionMap("Player").Disable();

        rebindingOperation = roll.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();

        startRebindObject.SetActive(true);
        waitingForInputObject.SetActive(false);

        //playerControler.PlayerInput.SwitchCurrentActionMap("Player");
        playerControler.PlayerInput.actions.FindActionMap("Menu").Disable();
        playerControler.PlayerInput.actions.FindActionMap("Player").Enable();
    }

    public void SetPlayer(PlayerControler pc)
    {
        playerControler = pc;
    }
}

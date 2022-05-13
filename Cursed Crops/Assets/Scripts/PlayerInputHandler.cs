using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public GameObject player;
    public PlayerConfiguration pConfig;
    private PlayerInputActions controls;
    public PlayerControler pc;
    public BuildingSystem bs;
    // Bools for the tutorial
    public bool allowAttack = true;
    public bool allowBuild = true;
    public bool allowMove = true;
    public bool dialogueIsHappening = false;
    public bool forDialogue = false;
    public bool attackOnce = false;
    public bool rollOnce = false;
    public bool shootOnce = false;
    private int counter = 0;


    void Start()
    {
        controls = new PlayerInputActions();
    }

    public void InitializePlayer(GameObject playerRef, PlayerConfiguration playerConfig)
    {
        player = playerRef;
        pConfig = playerConfig;
        pc = player.GetComponent<PlayerControler>();
        bs = player.GetComponent<BuildingSystem>();
        pc.input = pConfig.Input;
        pConfig.Input.onActionTriggered += Input_onActionTriggered;
    }

    public void UninitializePlayer()
    {
        pConfig.Input.onActionTriggered -= Input_onActionTriggered;
        player = null;
        pConfig = null;
        pc = null;
        bs = null;
    }

    private void Input_onActionTriggered(InputAction.CallbackContext context)
    {
        // =============== Player Controller ===============
        // Aiming
        if (context.action.name == controls.Player.Aim.name)
        {
            //Debug.Log("Handler: Calling aim");
            if (!dialogueIsHappening)
            {
                pc.Aim_performed(context);
            }
        }

        // Movement
        if (context.action.name == controls.Player.Movement.name)
        {
            //Debug.Log("Handler: Calling move");
            
             pc.Move_performed(context);
            
        }

        // Roll
        if (context.action.name == controls.Player.Roll.name)
        {
            //Debug.Log("Handler: Calling roll");
            if (allowAttack)
            {
                rollOnce = true;
                pc.Roll_performed(context);
            }
        }

        // Attack
        if (context.action.name == controls.Player.Attack.name)
        {
            //Debug.Log("Handler: Calling Attack");
            counter++;
            if (counter == 2)
            {
                forDialogue = true;
            }
            else if(counter > 2)
            {
                counter = 0;
            }
            if (!dialogueIsHappening && allowAttack)
            {
                attackOnce = true;
                pc.Attack_performed(context);
            }
        }

        // Ranged
        if (context.action.name == controls.Player.Ranged.name)
        {
            //Debug.Log("Handler: Calling Ranged");
            if (allowAttack)
            {
                shootOnce = true;
                pc.Ranged_performed(context);
            }
        }

        // Pause
        if (context.action.name == controls.Player.Pause.name)
        {
            //Debug.Log("Handler: Calling Ranged");
            pc.togglePause();
        }

        // =============== Player Controller ===============
        // Build Mode
        if (context.action.name == controls.Player.Build.name)
        {
            //Debug.Log("Handler: Calling Build Mode");
            if (allowBuild)
            {
                bs.BuildMode_performed(context);
            }
        }

        // Place
        if (context.action.name == controls.Player.Place.name)
        {
            //Debug.Log("Handler: Calling Place");
            bs.Place_performed(context);
        }

        // Destroy
        if (context.action.name == controls.Player.Destroy.name)
        {
            //Debug.Log("Handler: Calling Destroy");
            bs.Destroy_performed(context);
        }

        // Switch Left
        if (context.action.name == controls.Player.SwitchLeft.name)
        {
            //Debug.Log("Handler: Calling Switch Left");
            bs.SwitchLeft_performed(context);
        }

        // Switch Right
        if (context.action.name == controls.Player.Switchright.name)
        {
            //Debug.Log("Handler: Calling Switch Right");
            bs.SwitchRight_performed(context);
        }

        // Select East
        if (context.action.name == controls.Player.SelectEast.name)
        {
            //Debug.Log("Handler: Calling Select East");
            bs.SelectEast_performed(context);
        }

        // Select North
        if (context.action.name == controls.Player.SelectNorth.name)
        {
            //Debug.Log("Handler: Calling Select North");
            bs.SelectNorth_performed(context);
        }

        // Select South
        if (context.action.name == controls.Player.SelectSouth.name)
        {
            //Debug.Log("Handler: Calling Select South");
            bs.SelectSouth_performed(context);
        }

        // Select West
        if (context.action.name == controls.Player.SelectWest.name)
        {
            //Debug.Log("Handler: Calling Select West");
            bs.SelectWest_performed(context);
        }

    }
}

// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2204007d-045b-4c0c-8dcb-d5544a7a0447"",
            ""actions"": [
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""8bf8db31-ce2d-45f6-80f8-76cfc98af689"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ranged"",
                    ""type"": ""Button"",
                    ""id"": ""78ef1b08-176d-4b46-99eb-79f65b82c09c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""68cb8dc6-0c65-43ac-a1b2-e4767004a08f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""5a2a9781-0d80-4845-b311-2137b4ce8ed8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""063adcea-5308-4a76-9dbf-c0d3ffd4697e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Build"",
                    ""type"": ""Button"",
                    ""id"": ""aebf6278-7f77-4e36-b212-c40d770a7dab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""035c1447-4de8-4689-95bf-4240a09d8944"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Place"",
                    ""type"": ""Button"",
                    ""id"": ""db451421-19df-40d6-bb0d-29edad8b0be3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchLeft"",
                    ""type"": ""Button"",
                    ""id"": ""b4d67f51-4e9c-47ee-b63e-c8a86bb6826a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Switchright"",
                    ""type"": ""Button"",
                    ""id"": ""f23ff558-5b6d-4126-9b47-a5854c8b1e53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectNorth"",
                    ""type"": ""Button"",
                    ""id"": ""e4df2b50-bf09-4d37-98e4-c209761d7268"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectSouth"",
                    ""type"": ""Button"",
                    ""id"": ""b4ccd6e9-ca2c-4888-8520-973aec198f54"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectEast"",
                    ""type"": ""Button"",
                    ""id"": ""87081256-78cd-47e0-83e8-2d113025fc27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SelectWest"",
                    ""type"": ""Button"",
                    ""id"": ""58bad2c5-7786-441f-97ac-97d90ff44d7c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Swap"",
                    ""type"": ""Button"",
                    ""id"": ""f7d28228-f4c5-46f8-b819-fe898b157e29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""79170d67-a8d9-4ec4-8614-d35f8c4b0621"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""21bc74af-7427-478a-8cf9-526d5030e11e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33268ad3-218a-4000-af8d-fffb6b425c1a"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9702c04-1736-4861-b820-b76e7ad28010"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb0fc928-176d-487d-9f22-8ba7011ecae5"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ranged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""72bf5d23-8990-4f14-b339-aa49e0bbe826"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Ranged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77514d0c-2c0c-44b4-ac7e-dc65279e2206"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Ranged"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1c7a8238-317d-4197-a198-b2826274de46"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef2ca2fb-dec3-4b95-884d-c8fbe03c87c8"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc57f644-cfaa-4879-adbd-abb56ac08f37"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fc4bdbc5-9178-4ca1-a709-563829094a49"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9e3cdea3-726d-4c0a-a4ec-b0aa21710fde"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""404190ef-0ebc-4daf-b054-c8bea1c54bd7"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""b0340999-cbcf-496d-b1d5-8625943db51f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a234325c-7a9b-4762-80ca-7c1b6350b688"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fdfb524a-9ff3-4187-bcd2-84cb70ce9c84"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""22bdb14e-f250-44a5-a565-24ebe60ae27d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""c3929c6a-15bb-4756-b094-556c9abd9ca8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fbc313a7-c12c-4bd0-8615-fcbc1d9620ee"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4868961-f448-4500-9356-ccdedeb8834e"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fb27d8e-7c27-4673-be52-7883afb77e45"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ce173b8-ac61-4f2d-848a-a8b9b533fde3"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fec02a40-f4c2-4151-8923-5a92fb4a8dbc"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d302226-63bd-4e61-9e39-f4c303dcfe3c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""707d5924-04a5-4fc1-80aa-401d0ec98a02"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SwitchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e073b6a5-92c7-4cd2-9840-6f13eebd573e"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SwitchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9495236f-f906-4158-ad0a-02156d50d569"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Switchright"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe27cba7-8b00-468e-94b1-c5db7d4967a8"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Switchright"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""059b0c81-f58f-49af-81f2-86fec4ae4d3f"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c39a8918-fc2c-4c3d-afd9-6323b5eeeef4"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbe7df29-417c-40ce-85bd-7f523ca9e624"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfaaefdf-f859-4afb-8608-bacd7db16a1a"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d6b3f12-c5d7-4937-a8e4-8f8c45cae174"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d74ba80d-494c-47c6-84dc-7e4720007943"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3badffb8-e6a9-4e25-a92d-c576bd524aca"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""SelectWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""751e8e9d-89d1-490a-8443-c066fad21cff"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""SelectWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""36520a7e-0991-4333-a242-ae3f55e06d36"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""872c4919-74f2-41c7-ac01-d6ebabd3188e"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e593b96c-a267-4b59-b44d-a37c0e77a730"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Ranged = m_Player.FindAction("Ranged", throwIfNotFound: true);
        m_Player_Roll = m_Player.FindAction("Roll", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_Build = m_Player.FindAction("Build", throwIfNotFound: true);
        m_Player_Rotate = m_Player.FindAction("Rotate", throwIfNotFound: true);
        m_Player_Place = m_Player.FindAction("Place", throwIfNotFound: true);
        m_Player_SwitchLeft = m_Player.FindAction("SwitchLeft", throwIfNotFound: true);
        m_Player_Switchright = m_Player.FindAction("Switchright", throwIfNotFound: true);
        m_Player_SelectNorth = m_Player.FindAction("SelectNorth", throwIfNotFound: true);
        m_Player_SelectSouth = m_Player.FindAction("SelectSouth", throwIfNotFound: true);
        m_Player_SelectEast = m_Player.FindAction("SelectEast", throwIfNotFound: true);
        m_Player_SelectWest = m_Player.FindAction("SelectWest", throwIfNotFound: true);
        m_Player_Swap = m_Player.FindAction("Swap", throwIfNotFound: true);
        m_Player_Pause = m_Player.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Ranged;
    private readonly InputAction m_Player_Roll;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_Build;
    private readonly InputAction m_Player_Rotate;
    private readonly InputAction m_Player_Place;
    private readonly InputAction m_Player_SwitchLeft;
    private readonly InputAction m_Player_Switchright;
    private readonly InputAction m_Player_SelectNorth;
    private readonly InputAction m_Player_SelectSouth;
    private readonly InputAction m_Player_SelectEast;
    private readonly InputAction m_Player_SelectWest;
    private readonly InputAction m_Player_Swap;
    private readonly InputAction m_Player_Pause;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Ranged => m_Wrapper.m_Player_Ranged;
        public InputAction @Roll => m_Wrapper.m_Player_Roll;
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @Build => m_Wrapper.m_Player_Build;
        public InputAction @Rotate => m_Wrapper.m_Player_Rotate;
        public InputAction @Place => m_Wrapper.m_Player_Place;
        public InputAction @SwitchLeft => m_Wrapper.m_Player_SwitchLeft;
        public InputAction @Switchright => m_Wrapper.m_Player_Switchright;
        public InputAction @SelectNorth => m_Wrapper.m_Player_SelectNorth;
        public InputAction @SelectSouth => m_Wrapper.m_Player_SelectSouth;
        public InputAction @SelectEast => m_Wrapper.m_Player_SelectEast;
        public InputAction @SelectWest => m_Wrapper.m_Player_SelectWest;
        public InputAction @Swap => m_Wrapper.m_Player_Swap;
        public InputAction @Pause => m_Wrapper.m_Player_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Ranged.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRanged;
                @Ranged.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRanged;
                @Ranged.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRanged;
                @Roll.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRoll;
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Build.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBuild;
                @Build.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBuild;
                @Build.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBuild;
                @Rotate.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRotate;
                @Place.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlace;
                @Place.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlace;
                @Place.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPlace;
                @SwitchLeft.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchLeft;
                @SwitchLeft.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchLeft;
                @SwitchLeft.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchLeft;
                @Switchright.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchright;
                @Switchright.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchright;
                @Switchright.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwitchright;
                @SelectNorth.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectNorth;
                @SelectNorth.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectNorth;
                @SelectNorth.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectNorth;
                @SelectSouth.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectSouth;
                @SelectSouth.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectSouth;
                @SelectSouth.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectSouth;
                @SelectEast.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectEast;
                @SelectEast.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectEast;
                @SelectEast.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectEast;
                @SelectWest.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectWest;
                @SelectWest.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectWest;
                @SelectWest.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelectWest;
                @Swap.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwap;
                @Swap.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwap;
                @Swap.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSwap;
                @Pause.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Ranged.started += instance.OnRanged;
                @Ranged.performed += instance.OnRanged;
                @Ranged.canceled += instance.OnRanged;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Build.started += instance.OnBuild;
                @Build.performed += instance.OnBuild;
                @Build.canceled += instance.OnBuild;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Place.started += instance.OnPlace;
                @Place.performed += instance.OnPlace;
                @Place.canceled += instance.OnPlace;
                @SwitchLeft.started += instance.OnSwitchLeft;
                @SwitchLeft.performed += instance.OnSwitchLeft;
                @SwitchLeft.canceled += instance.OnSwitchLeft;
                @Switchright.started += instance.OnSwitchright;
                @Switchright.performed += instance.OnSwitchright;
                @Switchright.canceled += instance.OnSwitchright;
                @SelectNorth.started += instance.OnSelectNorth;
                @SelectNorth.performed += instance.OnSelectNorth;
                @SelectNorth.canceled += instance.OnSelectNorth;
                @SelectSouth.started += instance.OnSelectSouth;
                @SelectSouth.performed += instance.OnSelectSouth;
                @SelectSouth.canceled += instance.OnSelectSouth;
                @SelectEast.started += instance.OnSelectEast;
                @SelectEast.performed += instance.OnSelectEast;
                @SelectEast.canceled += instance.OnSelectEast;
                @SelectWest.started += instance.OnSelectWest;
                @SelectWest.performed += instance.OnSelectWest;
                @SelectWest.canceled += instance.OnSelectWest;
                @Swap.started += instance.OnSwap;
                @Swap.performed += instance.OnSwap;
                @Swap.canceled += instance.OnSwap;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnAttack(InputAction.CallbackContext context);
        void OnRanged(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnBuild(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnPlace(InputAction.CallbackContext context);
        void OnSwitchLeft(InputAction.CallbackContext context);
        void OnSwitchright(InputAction.CallbackContext context);
        void OnSelectNorth(InputAction.CallbackContext context);
        void OnSelectSouth(InputAction.CallbackContext context);
        void OnSelectEast(InputAction.CallbackContext context);
        void OnSelectWest(InputAction.CallbackContext context);
        void OnSwap(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Player/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Players"",
            ""id"": ""0d97fcce-0cc7-4580-a111-b8b0a345de0f"",
            ""actions"": [
                {
                    ""name"": ""Movement1"",
                    ""type"": ""Button"",
                    ""id"": ""47ca3d21-c86c-4f1a-8c4c-2fd0b17a10d9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump1"",
                    ""type"": ""Button"",
                    ""id"": ""bfc96f91-1018-4347-8546-17ecbaca770d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact1"",
                    ""type"": ""Button"",
                    ""id"": ""5635953c-10eb-4327-ad42-d3aadc760160"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PointUp1"",
                    ""type"": ""Button"",
                    ""id"": ""6f6e96c5-1213-4c60-a0c5-656ab04e9031"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Movement2"",
                    ""type"": ""Value"",
                    ""id"": ""91e8f5a3-cf9f-492c-97fa-5f71fb318db3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump2"",
                    ""type"": ""Button"",
                    ""id"": ""dc8e5b2b-a1cd-415a-93d2-b86c91ea06c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact2"",
                    ""type"": ""Button"",
                    ""id"": ""055c94f6-f173-4ac0-a5b0-700a5a867f29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PointUp2"",
                    ""type"": ""Button"",
                    ""id"": ""76bd7522-3026-4de7-ba09-f0204c7fccba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d1da139c-d3ca-42f4-8629-69a04ce25f2a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement1"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""88f38132-3166-4bf0-90c3-67d1dd38e267"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""80ebbf11-5d41-48c1-b678-32e889221f02"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""e6fc7ad8-21cb-452f-ab72-7791a69c72b4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Jump1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b4f8bf23-b536-434d-9784-052bba114bf2"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Interact1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7de44c8-e652-410e-a99d-155d8825992c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""PointUp1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b500f4a9-8350-4ba1-a860-ccccefa7e70b"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""PointUp2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""d4130506-aa56-4f8a-9073-d71e843234d1"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement2"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""48336d4f-8bcf-4644-85cf-53633757dc52"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""88bf8b29-2fd7-4e3e-b53b-93cb83d9a43b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Movement2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""2254daa3-cb1c-4a4c-8d7d-f165a35a3223"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Jump2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ef2c795-4b87-44a4-9db2-e3e7f5f14488"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player"",
                    ""action"": ""Interact2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player"",
            ""bindingGroup"": ""Player"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Players
        m_Players = asset.FindActionMap("Players", throwIfNotFound: true);
        m_Players_Movement1 = m_Players.FindAction("Movement1", throwIfNotFound: true);
        m_Players_Jump1 = m_Players.FindAction("Jump1", throwIfNotFound: true);
        m_Players_Interact1 = m_Players.FindAction("Interact1", throwIfNotFound: true);
        m_Players_PointUp1 = m_Players.FindAction("PointUp1", throwIfNotFound: true);
        m_Players_Movement2 = m_Players.FindAction("Movement2", throwIfNotFound: true);
        m_Players_Jump2 = m_Players.FindAction("Jump2", throwIfNotFound: true);
        m_Players_Interact2 = m_Players.FindAction("Interact2", throwIfNotFound: true);
        m_Players_PointUp2 = m_Players.FindAction("PointUp2", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Players
    private readonly InputActionMap m_Players;
    private IPlayersActions m_PlayersActionsCallbackInterface;
    private readonly InputAction m_Players_Movement1;
    private readonly InputAction m_Players_Jump1;
    private readonly InputAction m_Players_Interact1;
    private readonly InputAction m_Players_PointUp1;
    private readonly InputAction m_Players_Movement2;
    private readonly InputAction m_Players_Jump2;
    private readonly InputAction m_Players_Interact2;
    private readonly InputAction m_Players_PointUp2;
    public struct PlayersActions
    {
        private @PlayerControls m_Wrapper;
        public PlayersActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement1 => m_Wrapper.m_Players_Movement1;
        public InputAction @Jump1 => m_Wrapper.m_Players_Jump1;
        public InputAction @Interact1 => m_Wrapper.m_Players_Interact1;
        public InputAction @PointUp1 => m_Wrapper.m_Players_PointUp1;
        public InputAction @Movement2 => m_Wrapper.m_Players_Movement2;
        public InputAction @Jump2 => m_Wrapper.m_Players_Jump2;
        public InputAction @Interact2 => m_Wrapper.m_Players_Interact2;
        public InputAction @PointUp2 => m_Wrapper.m_Players_PointUp2;
        public InputActionMap Get() { return m_Wrapper.m_Players; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayersActions set) { return set.Get(); }
        public void SetCallbacks(IPlayersActions instance)
        {
            if (m_Wrapper.m_PlayersActionsCallbackInterface != null)
            {
                @Movement1.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnMovement1;
                @Movement1.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnMovement1;
                @Movement1.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnMovement1;
                @Jump1.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnJump1;
                @Jump1.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnJump1;
                @Jump1.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnJump1;
                @Interact1.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnInteract1;
                @Interact1.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnInteract1;
                @Interact1.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnInteract1;
                @PointUp1.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnPointUp1;
                @PointUp1.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnPointUp1;
                @PointUp1.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnPointUp1;
                @Movement2.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnMovement2;
                @Movement2.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnMovement2;
                @Movement2.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnMovement2;
                @Jump2.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnJump2;
                @Jump2.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnJump2;
                @Jump2.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnJump2;
                @Interact2.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnInteract2;
                @Interact2.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnInteract2;
                @Interact2.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnInteract2;
                @PointUp2.started -= m_Wrapper.m_PlayersActionsCallbackInterface.OnPointUp2;
                @PointUp2.performed -= m_Wrapper.m_PlayersActionsCallbackInterface.OnPointUp2;
                @PointUp2.canceled -= m_Wrapper.m_PlayersActionsCallbackInterface.OnPointUp2;
            }
            m_Wrapper.m_PlayersActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement1.started += instance.OnMovement1;
                @Movement1.performed += instance.OnMovement1;
                @Movement1.canceled += instance.OnMovement1;
                @Jump1.started += instance.OnJump1;
                @Jump1.performed += instance.OnJump1;
                @Jump1.canceled += instance.OnJump1;
                @Interact1.started += instance.OnInteract1;
                @Interact1.performed += instance.OnInteract1;
                @Interact1.canceled += instance.OnInteract1;
                @PointUp1.started += instance.OnPointUp1;
                @PointUp1.performed += instance.OnPointUp1;
                @PointUp1.canceled += instance.OnPointUp1;
                @Movement2.started += instance.OnMovement2;
                @Movement2.performed += instance.OnMovement2;
                @Movement2.canceled += instance.OnMovement2;
                @Jump2.started += instance.OnJump2;
                @Jump2.performed += instance.OnJump2;
                @Jump2.canceled += instance.OnJump2;
                @Interact2.started += instance.OnInteract2;
                @Interact2.performed += instance.OnInteract2;
                @Interact2.canceled += instance.OnInteract2;
                @PointUp2.started += instance.OnPointUp2;
                @PointUp2.performed += instance.OnPointUp2;
                @PointUp2.canceled += instance.OnPointUp2;
            }
        }
    }
    public PlayersActions @Players => new PlayersActions(this);
    private int m_PlayerSchemeIndex = -1;
    public InputControlScheme PlayerScheme
    {
        get
        {
            if (m_PlayerSchemeIndex == -1) m_PlayerSchemeIndex = asset.FindControlSchemeIndex("Player");
            return asset.controlSchemes[m_PlayerSchemeIndex];
        }
    }
    public interface IPlayersActions
    {
        void OnMovement1(InputAction.CallbackContext context);
        void OnJump1(InputAction.CallbackContext context);
        void OnInteract1(InputAction.CallbackContext context);
        void OnPointUp1(InputAction.CallbackContext context);
        void OnMovement2(InputAction.CallbackContext context);
        void OnJump2(InputAction.CallbackContext context);
        void OnInteract2(InputAction.CallbackContext context);
        void OnPointUp2(InputAction.CallbackContext context);
    }
}

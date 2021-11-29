// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/PlayerInp.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInp : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInp()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInp"",
    ""maps"": [
        {
            ""name"": ""InGameAP"",
            ""id"": ""44cba33b-2ec3-4a72-93aa-455c5b986d47"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""69595ed1-54d0-412d-9322-e6dd36047dcb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""3ec2d9da-e454-4f99-8366-ded65b6a7416"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""a685b94f-040e-447d-b0b8-75ea0855a691"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Bark"",
                    ""type"": ""Button"",
                    ""id"": ""9af4ac93-4b8f-4f51-9b6b-016efeaa723b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""7ccedf96-0466-4a4d-bb53-e461a91d3197"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Accept"",
                    ""type"": ""Button"",
                    ""id"": ""78ea4ee5-4960-47db-a369-bf0b824c164a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""519459e5-f113-47ae-8917-7794c340a545"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""88f737e0-35d5-4726-a128-9973e8579d2f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bc6bf8c9-a571-48d3-b54b-0ab9f3499dce"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""bce89307-4acf-4995-b405-22f1eade5884"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e23e624d-e052-47a7-9ed5-78bc3f450613"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b02d05c5-1f07-4bae-b7df-4106be49cf04"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b723dde6-f28e-40ed-ae74-00497fa6da93"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50f37e3e-6ee3-4c66-8cca-6b6abf0ea6c3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0c8412b-a27c-4f18-88b1-bf69b14fd8e9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49622089-03c1-4a7c-85e7-1dcdb83d9b4e"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c482ab63-5cb5-40c3-a53b-7205b7ad818b"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""381dc49f-72cf-4cc3-aca3-7146d0d9d181"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3287ed44-11c8-40f2-b5a6-08ee2ccf8df1"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfa420f2-8633-49b7-8b66-a7e98aad5938"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9bc9622-67f4-46ed-b5be-d4134aeb0c29"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f6b575a-cbf3-45ab-8e7d-953612775006"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Bark"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfbaafeb-f920-4daf-95e9-aa5b96e010d1"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aea97162-66f9-4744-9c69-88b915672f92"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41591542-2976-4622-9f80-1432cd0f42c9"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3416ca5-624e-4f60-b6ed-7421130659a5"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b09cfef5-db0f-4a41-8f53-7cebfebc11a5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4c2caa8-cb28-459f-9cae-fcddbee73593"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // InGameAP
        m_InGameAP = asset.FindActionMap("InGameAP", throwIfNotFound: true);
        m_InGameAP_Move = m_InGameAP.FindAction("Move", throwIfNotFound: true);
        m_InGameAP_Dash = m_InGameAP.FindAction("Dash", throwIfNotFound: true);
        m_InGameAP_LightAttack = m_InGameAP.FindAction("LightAttack", throwIfNotFound: true);
        m_InGameAP_Bark = m_InGameAP.FindAction("Bark", throwIfNotFound: true);
        m_InGameAP_Cancel = m_InGameAP.FindAction("Cancel", throwIfNotFound: true);
        m_InGameAP_Accept = m_InGameAP.FindAction("Accept", throwIfNotFound: true);
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

    // InGameAP
    private readonly InputActionMap m_InGameAP;
    private IInGameAPActions m_InGameAPActionsCallbackInterface;
    private readonly InputAction m_InGameAP_Move;
    private readonly InputAction m_InGameAP_Dash;
    private readonly InputAction m_InGameAP_LightAttack;
    private readonly InputAction m_InGameAP_Bark;
    private readonly InputAction m_InGameAP_Cancel;
    private readonly InputAction m_InGameAP_Accept;
    public struct InGameAPActions
    {
        private @PlayerInp m_Wrapper;
        public InGameAPActions(@PlayerInp wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_InGameAP_Move;
        public InputAction @Dash => m_Wrapper.m_InGameAP_Dash;
        public InputAction @LightAttack => m_Wrapper.m_InGameAP_LightAttack;
        public InputAction @Bark => m_Wrapper.m_InGameAP_Bark;
        public InputAction @Cancel => m_Wrapper.m_InGameAP_Cancel;
        public InputAction @Accept => m_Wrapper.m_InGameAP_Accept;
        public InputActionMap Get() { return m_Wrapper.m_InGameAP; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameAPActions set) { return set.Get(); }
        public void SetCallbacks(IInGameAPActions instance)
        {
            if (m_Wrapper.m_InGameAPActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnMove;
                @Dash.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnDash;
                @LightAttack.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnLightAttack;
                @Bark.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnBark;
                @Bark.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnBark;
                @Bark.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnBark;
                @Cancel.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnCancel;
                @Accept.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnAccept;
                @Accept.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnAccept;
                @Accept.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnAccept;
            }
            m_Wrapper.m_InGameAPActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @Bark.started += instance.OnBark;
                @Bark.performed += instance.OnBark;
                @Bark.canceled += instance.OnBark;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Accept.started += instance.OnAccept;
                @Accept.performed += instance.OnAccept;
                @Accept.canceled += instance.OnAccept;
            }
        }
    }
    public InGameAPActions @InGameAP => new InGameAPActions(this);
    public interface IInGameAPActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnBark(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnAccept(InputAction.CallbackContext context);
    }
}

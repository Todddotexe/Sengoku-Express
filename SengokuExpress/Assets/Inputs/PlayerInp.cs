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
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""2eb5ac65-c3b7-4f76-ba64-51307d0538b9"",
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
                    ""id"": ""28b33c08-b531-4570-a8a1-ca83df786261"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
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
                    ""id"": ""7e9e4510-893e-47cd-8edb-de48afb7dccd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
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
        m_InGameAP_HeavyAttack = m_InGameAP.FindAction("HeavyAttack", throwIfNotFound: true);
        m_InGameAP_Bark = m_InGameAP.FindAction("Bark", throwIfNotFound: true);
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
    private readonly InputAction m_InGameAP_HeavyAttack;
    private readonly InputAction m_InGameAP_Bark;
    public struct InGameAPActions
    {
        private @PlayerInp m_Wrapper;
        public InGameAPActions(@PlayerInp wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_InGameAP_Move;
        public InputAction @Dash => m_Wrapper.m_InGameAP_Dash;
        public InputAction @LightAttack => m_Wrapper.m_InGameAP_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_InGameAP_HeavyAttack;
        public InputAction @Bark => m_Wrapper.m_InGameAP_Bark;
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
                @HeavyAttack.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnHeavyAttack;
                @HeavyAttack.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnHeavyAttack;
                @HeavyAttack.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnHeavyAttack;
                @Bark.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnBark;
                @Bark.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnBark;
                @Bark.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnBark;
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
                @HeavyAttack.started += instance.OnHeavyAttack;
                @HeavyAttack.performed += instance.OnHeavyAttack;
                @HeavyAttack.canceled += instance.OnHeavyAttack;
                @Bark.started += instance.OnBark;
                @Bark.performed += instance.OnBark;
                @Bark.canceled += instance.OnBark;
            }
        }
    }
    public InGameAPActions @InGameAP => new InGameAPActions(this);
    public interface IInGameAPActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
        void OnBark(InputAction.CallbackContext context);
    }
}

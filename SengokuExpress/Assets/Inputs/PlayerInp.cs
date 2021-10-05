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
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""69595ed1-54d0-412d-9322-e6dd36047dcb"",
                    ""expectedControlType"": ""Vector2"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
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
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // InGameAP
        m_InGameAP = asset.FindActionMap("InGameAP", throwIfNotFound: true);
        m_InGameAP_Movement = m_InGameAP.FindAction("Movement", throwIfNotFound: true);
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
    private readonly InputAction m_InGameAP_Movement;
    public struct InGameAPActions
    {
        private @PlayerInp m_Wrapper;
        public InGameAPActions(@PlayerInp wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_InGameAP_Movement;
        public InputActionMap Get() { return m_Wrapper.m_InGameAP; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(InGameAPActions set) { return set.Get(); }
        public void SetCallbacks(IInGameAPActions instance)
        {
            if (m_Wrapper.m_InGameAPActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_InGameAPActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_InGameAPActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public InGameAPActions @InGameAP => new InGameAPActions(this);
    public interface IInGameAPActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
}

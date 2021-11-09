using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

[Serializable]
public class InputMoveEvent : UnityEvent<float, float> { }
[Serializable]
public class InputDashEvent : UnityEvent<bool> { }
[Serializable]
public class InputLightAttackEvent : UnityEvent<bool> { }
[Serializable]
public class InputBarkEvent : UnityEvent<bool> { }

public class InputCon : MonoBehaviour
{
    PlayerInp cont;
    public InputMoveEvent inputMoveEv;
    public InputDashEvent inputDashEv;
    public InputLightAttackEvent inputLightAttackEv;
    public InputBarkEvent inputBarkEv;

    private void Awake()
    {
        cont = new PlayerInp();
    }
    private void OnEnable()
    {
        cont.InGameAP.Enable();
        cont.InGameAP.Move.performed += OnMovePerform;
        cont.InGameAP.Move.canceled += OnMovePerform;
        cont.InGameAP.Dash.performed += OnDashPerform;
        cont.InGameAP.LightAttack.performed += OnLightPerform;
        cont.InGameAP.Bark.performed += OnBarkSelectPerform;
    }
    public void OnMovePerform(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        inputMoveEv.Invoke(moveInput.x, moveInput.y);
    }
    public void OnDashPerform(InputAction.CallbackContext context)
    {
        bool dashPerform = context.ReadValueAsButton();
        inputDashEv.Invoke(dashPerform);
    }
    public void OnLightPerform(InputAction.CallbackContext context)
    {
        bool lightAtkInput = context.ReadValueAsButton();
        inputLightAttackEv.Invoke(lightAtkInput);
    }
    public void OnBarkSelectPerform(InputAction.CallbackContext context)
    {
        bool barkInput = context.ReadValueAsButton();
        inputBarkEv.Invoke(barkInput);
    }
}


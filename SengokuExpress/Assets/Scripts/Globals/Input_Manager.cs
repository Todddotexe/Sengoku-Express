using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour {
    public PlayerInput input                 = null;
    public string INPUT_LABEL_CANCEL         = "Cancel";
    public string INPUT_LABEL_ACCEPT         = "Accept";
    public string INPUT_LABEL_NAVIGATION     = "Move";
    [HideInInspector] InputAction cancel     = null;
    [HideInInspector] InputAction accept     = null;
    [HideInInspector] InputAction navigation = null;

    void Awake() {
        Global.input_manager = this;
    }
    
    void Start() {
        Debug.Assert(input != null); // we should have assigned this through the editor
        cancel     = input.actions[INPUT_LABEL_CANCEL];
        accept     = input.actions[INPUT_LABEL_ACCEPT];
        navigation = input.actions[INPUT_LABEL_NAVIGATION];
        
        // -- pause when we press the cancel button when it's appropriate
        cancel.performed += delegate_on_pause_button_pressed;
    }
    ///
    void delegate_on_pause_button_pressed(InputAction.CallbackContext obj) {
        // if we're not paused, set state to paused, 
        switch (Global.get_state()) {
            case Global.STATES.GAME:
                Global.set_game_state(Global.STATES.PAUSED);
            break;
            case Global.STATES.PAUSED:
                Global.set_game_state(Global.STATES.GAME);
            break;
        }
    }
}

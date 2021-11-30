using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class Input_Manager : MonoBehaviour {
    public PlayerInput input                 = null;
    public EventSystem event_system          = null;
    public GameObject main_menu_initial_selected     = null;
    public GameObject pause_menu_initial_selected    = null;
    public GameObject settings_menu_initial_selected = null;
    public GameObject lost_menu_initial_selected     = null;
    public GameObject win_menu_initial_selected      = null;
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
        Debug.Assert(input        != null); // we should have assigned this through the editor
        Debug.Assert(event_system != null); // we should have assigned this through the editor
        // cancel.RemoveAction();
        cancel     = input.actions[INPUT_LABEL_CANCEL];
        accept     = input.actions[INPUT_LABEL_ACCEPT];
        navigation = input.actions[INPUT_LABEL_NAVIGATION];
        
        // -- pause when we press the cancel button when it's appropriate
        // ! REMINDER, ON SOME MACHINES, WE NEED THIS FLAG HERE BECAUSE IT'LL PREVENT THE DELEGATE FROM BEING ADDED TWICE, ON SOME MACHINES, UNITY HANDLES THIS FINE AND WITH THIS FLAG HERE WE WOULDN'T HAVE ADDED THE DELEGATE EVEN ONCE.
        // if (!Global.has_init_input_manager) { // ! we need to check for this, otherwise cancel.performed will call delegate_on_pause_button_pressed twice. WE NEED TO RUN THE FOLLOWING PROCEDURE ONLY ONCE
            // cancel.performed += delegate_on_pause_button_pressed;
            // Global.has_init_input_manager = true; // ! set Global.has_init to true
        // }
    }
    ///
    void Update() {
        if (event_system.currentSelectedGameObject == null) {
            switch (Global.get_gui_state()) {
                case GUI_Controller.PANELS.GAME: {
                    event_system.SetSelectedGameObject(main_menu_initial_selected);
                } break;
                case GUI_Controller.PANELS.PAUSE: {
                    event_system.SetSelectedGameObject(pause_menu_initial_selected);
                } break;
                case GUI_Controller.PANELS.LOST: {
                    event_system.SetSelectedGameObject(lost_menu_initial_selected);
                } break;
                case GUI_Controller.PANELS.WIN: {
                    event_system.SetSelectedGameObject(win_menu_initial_selected);
                } break;
            }
        }

        // -- check for escape (to pause)
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame) {
            delegate_on_pause_button_pressed();
        }
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) {
            delegate_on_pause_button_pressed();
        }
    }
    ///
    // void delegate_on_pause_button_pressed(InputAction.CallbackContext obj) {
    //     // if we're not paused, set state to paused, 
    //     switch (Global.get_state()) {
    //         case Global.STATES.GAME:
    //             Global.set_game_state(Global.STATES.PAUSED);
    //         break;
    //         case Global.STATES.PAUSED:
    //             Global.set_game_state(Global.STATES.GAME);
    //         break;
    //     }
    // }
    void delegate_on_pause_button_pressed() {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GUI_Controller : MonoBehaviour {
    public enum PANELS {
        LOST, WIN, GAME, PAUSE,
    }
    // !== public delegates the Unity Editor Buttons can use ==! //
    public RectTransform lostPanel = null;
    public RectTransform winPanel = null;
    public RectTransform gamePanel = null;
    public RectTransform pausePanel = null;
    public GUI_Game      game_gui = null;

    // !== public methods not meant to be used by the Unity Editor ==! //
    public void switch_panel(PANELS panel) {
        switch (panel) {
            case PANELS.LOST: {
                if (lostPanel != null) lostPanel.gameObject.SetActive (true);
                if (winPanel  != null) winPanel.gameObject.SetActive  (false);
                if (gamePanel != null) gamePanel.gameObject.SetActive (false);
                if (gamePanel != null) pausePanel.gameObject.SetActive(false);
            } break;
            case PANELS.WIN: {
                if (lostPanel != null) lostPanel.gameObject.SetActive (false);
                if (winPanel  != null) winPanel.gameObject.SetActive  (true);
                if (gamePanel != null) gamePanel.gameObject.SetActive (false);
                if (gamePanel != null) pausePanel.gameObject.SetActive(false);
            } break;
            case PANELS.GAME: {
                if (lostPanel != null) lostPanel.gameObject.SetActive (false);
                if (winPanel  != null) winPanel.gameObject.SetActive  (false);
                if (gamePanel != null) gamePanel.gameObject.SetActive (true);
                if (gamePanel != null) pausePanel.gameObject.SetActive(false);
            } break;
            case PANELS.PAUSE: {
                if (lostPanel != null) lostPanel.gameObject.SetActive (false);
                if (winPanel  != null) winPanel.gameObject.SetActive  (false);
                if (gamePanel != null) gamePanel.gameObject.SetActive (false);
                if (gamePanel != null) pausePanel.gameObject.SetActive(true);
            } break;
        }
    }

    // !== MAIN METHODS !== //
    /// on awake
    void Awake() {
        Global.set_gui(this);
    }
    /// on level start
    void Start() {
        switch_panel(PANELS.GAME);
        Debug.Assert(game_gui != null); // @debug potentially a problem, I forget where this is meant to be assigned
        Global.set_game_state(Global.STATES.GAME);
    }
    ///
    public void set_bark_meter(float value) {
        Mathf.Clamp(value, 0, 1);
        game_gui.bark_meter.value = value;
    }
    ///
    public void set_health(int health) {
        Mathf.Clamp(health, 0, 6);
        int max_health = 10; // @incomplete put this variable in a proper place
        game_gui.hp_board_material.SetFloat("inverse_health", ((float)health / (float)max_health));
    }
    ///
    public void gui_display_pause_menu() {
        switch_panel(PANELS.PAUSE);
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

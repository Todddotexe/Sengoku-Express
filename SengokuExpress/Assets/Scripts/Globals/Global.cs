using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class Global {
    /// tags
    public static string tag_environment = "environment";
    public static string tag_player = "Player";
    public static Enemy_Blackboard blackboard = new Enemy_Blackboard();
    public static Input_Manager input_manager = null;
    /// layers
    /// pause system
    static GUI_Controller gui = null;
    private static float pre_pause_time_scale = 1; // ! used to reset time scale to what it was before pausing the game, in case we changed it for slow mo effects
    private static void set_pause(bool value) { // use set state to set pause outside of Global.cs scope
        if (value) {
            pre_pause_time_scale = Time.timeScale;
            Time.timeScale = 0;
            gui.gui_display_pause_menu();
        }
        else {
            Time.timeScale = pre_pause_time_scale;
        }
    }
    /// set the gui
    public static void set_gui(GUI_Controller _gui) {
        gui = _gui;
    }
    /// set game state
    public enum STATES {
        GAME, WIN, LOST, PAUSED,
    }
    static STATES state = STATES.GAME;
    public static STATES get_state() {return state;}

    public static void set_game_state(STATES _state) {
        state = _state;
        switch (_state) {
            case STATES.GAME: {
                gui.switch_panel(GUI_Controller.PANELS.GAME);
                set_pause(false);
            } break;
            case STATES.WIN:  {
                gui.switch_panel(GUI_Controller.PANELS.WIN);
                set_pause(true);
            } break;
            case STATES.LOST: {
                gui.switch_panel(GUI_Controller.PANELS.LOST);
                set_pause(true);
            } break;
            case STATES.PAUSED: {
                gui.switch_panel(GUI_Controller.PANELS.PAUSE);
                set_pause(true);
            } break;
        }
    }

    public static void set_bark_meter(float value) {
        gui.set_bark_meter(value);
    }

    public static void set_health(int value) { // TODO remove this after // @debug & // @test
        gui.set_health(value);
    }
    /// restart level
    static public void restart_level() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gui.switch_panel(GUI_Controller.PANELS.GAME);
    }
    /// go to main menu
    static private int mainMenuSceneIndex = 0;
    static public void go_to_main_menu() {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
    /// tags
    public static string tag_environment = "environment";
    public static string tag_player = "Player";
    public static Enemy_Blackboard blackboard = new Enemy_Blackboard();
    /// layers
    /// pause system
    static GUI_Controller gui = null;
    private static float pre_pause_time_scale = 1; // ! used to reset time scale to what it was before pausing the game, in case we changed it for slow mo effects
    public static void set_pause(bool value) {
        if (value) {
            pre_pause_time_scale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = pre_pause_time_scale;
    }
    /// set the gui
    public static void set_gui(GUI_Controller _gui) {
        gui = _gui;
    }
    /// set game state
    public enum STATES {
        GAME, WIN, LOST, PAUSED,
    }
    public static void set_game_state(STATES state) {
        switch (state) {
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

    public static void set_health_text(int value) { // TODO remove this after // @debug & // @test
        gui.set_health_text(value);
    }
}
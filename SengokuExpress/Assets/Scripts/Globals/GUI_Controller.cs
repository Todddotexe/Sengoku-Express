using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUI_Controller : MonoBehaviour {
    public enum PANELS {
        LOST, WIN, GAME,
    }
    // !== public delegates the Unity Editor Buttons can use ==! //
    public int mainMenuSceneIndex = 0;
    public RectTransform lostPanel = null;
    public RectTransform winPanel = null;
    public RectTransform gamePanel = null;
    /// restart level
    public void restart_level() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        switch_panel(PANELS.GAME);
    }
    /// go to main menu
    public void go_to_main_menu() {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
    /// resume the game if paused
    public void resume_game() {
        Global.set_pause(false);
        switch_panel(PANELS.GAME);
    }

    // !== public methods not meant to be used by the Unity Editor ==! //
    public void switch_panel(PANELS panel) {
        switch (panel) {
            case PANELS.LOST: {
                if (lostPanel != null) lostPanel.gameObject.SetActive(true);
                if (winPanel  != null) lostPanel.gameObject.SetActive(false);
                if (gamePanel != null) lostPanel.gameObject.SetActive(false);
            } break;
            case PANELS.WIN: {
                if (lostPanel != null) lostPanel.gameObject.SetActive(false);
                if (winPanel  != null) lostPanel.gameObject.SetActive(true);
                if (gamePanel != null) lostPanel.gameObject.SetActive(false);
            } break;
            case PANELS.GAME: {
                if (lostPanel != null) lostPanel.gameObject.SetActive(false);
                if (winPanel  != null) lostPanel.gameObject.SetActive(false);
                if (gamePanel != null) lostPanel.gameObject.SetActive(true);
            } break;
        }
    }

    // !== MAIN METHODS !== //
    /// on level start
    void Start() {
        switch_panel(PANELS.GAME);
        Global.set_pause(false);
    }

}

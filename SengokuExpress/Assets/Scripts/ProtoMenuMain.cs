using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoMenuMain : MonoBehaviour
{
    public AudioSource playSound;
    float timer_init = .75f;
    float timer = .75f;
    bool want_to_play = false;
    void Update() {
        if (want_to_play) {
            if (!playSound.isPlaying) playSound.Play();
            if (timer > 0) {
                timer -= Time.unscaledDeltaTime;
            } else {
                timer = timer_init;
                SceneManager.LoadScene("level_main_game");
            }

        }
    }
    public void loadPlay()
    {
        want_to_play = true;
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("proto_main_menu");
    }
    public void loadSettings()
    {
        SceneManager.LoadScene("proto_settings_menu");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

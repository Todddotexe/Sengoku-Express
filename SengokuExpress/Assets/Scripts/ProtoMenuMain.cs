using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoMenuMain : MonoBehaviour
{
    public AudioSource playSound;
    public void loadPlay()
    {
        playSound.Play();
        StartCoroutine (loadPlayScene(.75f));
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
    IEnumerator loadPlayScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("greybox_001-map");

    }
}

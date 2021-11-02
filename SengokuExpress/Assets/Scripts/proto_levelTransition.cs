using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class proto_levelTransition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "player")
        {
            LoadNextLevel();

        }
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

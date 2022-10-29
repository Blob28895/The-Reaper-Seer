using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameoverUI;

    public void RestartButton()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("Main Menu");
    }
}

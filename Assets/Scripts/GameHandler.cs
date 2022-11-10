using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Game object that can be used to handle certain events like game overs or pausing
public class GameHandler : MonoBehaviour
{    // The number of seconds to wait before reading button down
    public float inputDelay = 1f;
    public GameOverScreen GameOverScreen;
    public GameObject gameoverUI;

    // Function to end the game when the Reaper reaches 0 hp
    public void GameOver()
    {
        gameoverUI.SetActive(true);
        Debug.Log("Game Over Screen");

        // Restart goes here
        StartCoroutine(RestartRoutine());
    }

    IEnumerator RestartRoutine()
    {
        yield return new WaitForSeconds(inputDelay);
        // Will wait until the jump key is pressed
        while (!Input.GetButtonDown("Jump"))
        {
            yield return null;
        }
        // After the space key is pressed, the scene is reloaded
        staticVariables.currHealth = Player.maxHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Scene Reloaded!");
    }
}

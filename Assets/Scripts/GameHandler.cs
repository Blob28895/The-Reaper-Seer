using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Game object that can be used to handle certain events like game overs or pausing
public class GameHandler : MonoBehaviour
{
    // The number of seconds to wait before reading button down
    public float inputDelay = 1f;
    
    // Function to end the game when the Reaper reaches 0 hp
    public void GameOver()
    {
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("Scene Reloaded!");
    }
}
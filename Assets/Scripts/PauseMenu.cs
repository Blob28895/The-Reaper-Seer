using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject gameOver;
    public GameObject controls;
    private PauseGamepadKeyboardNavigation nav;

    // Start is called before the first frame update
    private void Start()
    {
        GameIsPaused = false;
        nav = GetComponent<PauseGamepadKeyboardNavigation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !gameOver.activeInHierarchy)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (nav != null)
        {
            nav.ResetSelected();
        }
        pauseMenuUI.SetActive(false);
        controls.SetActive(false);
        Time.timeScale = 1f;
        InputSystem.ResumeHaptics();
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        InputSystem.PauseHaptics();
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        InputSystem.ResetHaptics();
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        InputSystem.ResetHaptics();
        Application.Quit();        
    }
}

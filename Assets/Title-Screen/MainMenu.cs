using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public void PlayGame()
    {
        //staticVariables.currHealth = Player.maxHealth;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(StartGameRoutine());
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    IEnumerator StartGameRoutine()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        // Done to get the fade effect to work correctly
        //SceneManager.LoadScene("Level 1");
        staticVariables.currHealth = Player.maxHealth;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}

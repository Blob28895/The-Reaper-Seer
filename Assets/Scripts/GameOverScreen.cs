using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameoverUI;
    private Animator fadeAnimator;
    public float transitionTime = 1f;

    public void RestartButton()
    {
        StartCoroutine(TransitionScene(SceneManager.GetActiveScene().name));
        //staticVariables.currHealth = Player.maxHealth;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HomeButton()
    {
        StartCoroutine(TransitionScene("Main Menu"));
        //SceneManager.LoadScene("Main Menu");
    }

    public void ScientistFight()
    {
        StartCoroutine(TransitionScene("ScientistFight"));
        //staticVariables.currHealth = Player.maxHealth;
        //SceneManager.LoadScene("ScientistFight");
    }

    IEnumerator TransitionScene(string scene)
    {
        GameObject fade = GameObject.FindWithTag("Fade");
        fadeAnimator = fade.GetComponent<Animator>();
        fadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        staticVariables.currHealth = Player.maxHealth;
        InputSystem.ResetHaptics();
        staticVariables.currSouls = 0;
        SceneManager.LoadScene(scene);
    }
}
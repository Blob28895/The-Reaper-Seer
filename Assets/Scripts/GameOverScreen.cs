using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameoverUI;
    private Animator fadeAnimator;
    public float transitionTime = 1f;

    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index = 0;
    public float typingSpeed;

    public void RestartButton()
    {
        StartCoroutine(TransitionScene(SceneManager.GetActiveScene().name));
        //staticVariables.currHealth = Player.maxHealth;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HomeButton()
    {
        staticVariables.ResetUpgrades();
        staticVariables.ResetStatics();
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
        if(scene == SceneManager.GetActiveScene().name)
		{
            Debug.Log("Restarting");
            NextSentence();
            yield return new WaitForSeconds(3f);
            StartCoroutine(ReverseType());
            yield return new WaitForSeconds(3f);
		}
        staticVariables.currHealth = Player.maxHealth;
        InputSystem.ResetHaptics();
        staticVariables.currSouls = 0;
        SceneManager.LoadScene(scene);
    }

    IEnumerator Type()
    {
        //Debug.Log(sentences[index]);
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(3f);
        index = 0;

    }
    IEnumerator ReverseType()
	{
        foreach (char letter in textDisplay.text.ToCharArray())
		{
            textDisplay.text = textDisplay.text.Substring(0, textDisplay.text.Length - 1);
            yield return new WaitForSeconds(typingSpeed);
		}
        yield return new WaitForSeconds(3f);
	}
    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            textDisplay.text = "";
            StartCoroutine(Type());
            index++;
        }
    }
}
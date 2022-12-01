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
    public float typingSpeed = 0.06f;
    private string sentence = "...wait...that's not how it went...";


    public void Start()
	{
        textDisplay = GameObject.FindGameObjectWithTag("MidText").GetComponent<TextMeshProUGUI>();
        
	}
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
            StartCoroutine(Type());
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
        //foreach (char letter in sentences[index].ToCharArray())

        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(0.06f);
        }
        yield return new WaitForSeconds(3f);
        index = 0;

    }
    IEnumerator ReverseType()
	{
        foreach (char letter in textDisplay.text.ToCharArray())
		{
            textDisplay.text = textDisplay.text.Substring(0, textDisplay.text.Length - 1);
            yield return new WaitForSeconds(0.06f);
		}
        yield return new WaitForSeconds(3f);
	}
    public void NextSentence()
    {
        Debug.Log(index);
        Debug.Log(sentences[0]);
        if (index < sentences.Length - 1)
        {
            Debug.Log("Made it into nextSentence if statement");
            textDisplay.text = "";
            StartCoroutine(Type());
            index++;
        }
    }
}
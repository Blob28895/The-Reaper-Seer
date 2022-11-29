using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    private bool ready = true;
    public float typingSpeed;
    public float transitionTime = 1f;

    void Start()
    {
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        ready = false;
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        ready = true;
    }

    IEnumerator TransitionScene()
    {
        GameObject fade = GameObject.FindWithTag("Fade");
        Animator fadeAnimator = fade.GetComponent<Animator>();
        fadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        staticVariables.currHealth = Player.maxHealth;
        if(SceneManager.GetActiveScene().name != "ConclusionDialogue")
		{
            Debug.Log("Normal Transition");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
		{
            Debug.Log("special transition");
            SceneManager.LoadScene(1);
		}
        
    }

    public void NextSentence()
    {
        if (index < sentences.Length -1 && ready)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else if (index >= sentences.Length - 1)
        {
            //textDisplay.text = "";
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            StartCoroutine(TransitionScene());
        }
    }
    
    public void NextScene()
	{
        StartCoroutine(TransitionScene());
	}
}

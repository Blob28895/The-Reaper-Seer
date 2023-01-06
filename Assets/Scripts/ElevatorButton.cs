using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
public class ElevatorButton : MonoBehaviour
{
	private bool touching = false;
	public GameObject Keycard;
	public Animator fadeAnimator;
	public float transitionTime = 1f;
	public TextMeshProUGUI textDisplay;
	public string[] sentences;
	private int index = 0;
	public float typingSpeed;

	// Deprecated code that used to be in OnTriggerEnter2D and OnTriggerStay2D functions
	/*if (transition && collision.name == "Reaper")
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			transition = false;
			StartCoroutine(TransitionScene());
		}
		else if (Input.GetButtonDown("Interact") && collision.name == "Reaper" && Keycard.activeSelf == false)
		{
			NextSentence();
		}*/

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Reaper")
        {
			touching = true;
		}
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.name == "Reaper")
		{
			touching = false;
		}
    }
    public IEnumerator TransitionScene()
    {
		fadeAnimator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		InputSystem.ResetHaptics();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Interact") && touching)
		{
			if (Keycard.activeSelf == true)
            {
				StartCoroutine(TransitionScene());
			}
			else
            {
				NextSentence();
			}
		}
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
		textDisplay.text = "";
		index = 0;
	}
	public void NextSentence()
	{
		if (index < sentences.Length)
		{
			textDisplay.text = "";
			StartCoroutine(Type());
			index++;
		}
	}
}

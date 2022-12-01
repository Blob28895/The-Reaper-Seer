using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ElevatorButton : MonoBehaviour
{
	private bool transition = false;
	public GameObject Keycard;
	public Animator fadeAnimator;
	public float transitionTime = 1f;
	public TextMeshProUGUI textDisplay;
	public string[] sentences;
	private int index = 0;
	public float typingSpeed;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (transition && collision.name == "Reaper")
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			transition = false;
			StartCoroutine(TransitionScene());
		}
		else if (Input.GetButtonDown("Interact") && collision.name == "Reaper" && Keycard.activeSelf == false)
		{
			NextSentence();
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (transition && collision.name == "Reaper")
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			transition = false;
			StartCoroutine(TransitionScene());
		}
		else if(Input.GetButtonDown("Interact") && collision.name == "Reaper" && Keycard.activeSelf == false)
		{
			NextSentence();
		}
	}
	public IEnumerator TransitionScene()
    {
		fadeAnimator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonDown("Interact") && Keycard.activeSelf == true)
		{
			transition = true;
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
		NextSentence();
		index = 0;

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

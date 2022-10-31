using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ElevatorButton : MonoBehaviour
{
	private bool transition = false;
	public GameObject Keycard;
	public Animator fadeAnimator;
	public float transitionTime = 1f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (transition && collision.name == "Reaper")
		{
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			transition = false;
			StartCoroutine(TransitionScene());
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
	}
	IEnumerator TransitionScene()
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
}

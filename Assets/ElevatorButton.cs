using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ElevatorButton : MonoBehaviour
{
	public Animator animator;
	public float transitionTime = 1f;
	private bool transition = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(transition && collision.name == "Reaper")
		{
			StartCoroutine(TransitionToScientistFight());
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (transition && collision.name == "Reaper")
		{
			StartCoroutine(TransitionToScientistFight());
		}
	}
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact"))
		{
			transition = true;
		}
    }

	IEnumerator TransitionToScientistFight()
    {
		enabled = false;
		animator.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		transition = false;
	}
}

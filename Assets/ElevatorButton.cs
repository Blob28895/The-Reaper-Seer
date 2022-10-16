using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ElevatorButton : MonoBehaviour
{
	private bool transition = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(transition && collision.name == "Reaper")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			transition = false;
		}
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (transition && collision.name == "Reaper")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			transition = false;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    public float startDelay;
    // Start is called before the first frame update
    void Start()
    {
        //startDelay = (float)GetComponent<VideoPlayer>().clip.length;
        StartCoroutine(LoadGameRoutine());
    }

    IEnumerator LoadGameRoutine()
    {
        yield return new WaitForSeconds(startDelay);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Main Menu");
    }
}

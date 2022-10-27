using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasScript : MonoBehaviour
{
    public float startDelay = 4f;
    public float gasDuration = 8f;
    private GameObject myself;
    private GameObject activeGas;
    private GameObject gasWarning;
    // Start is called before the first frame updat
    void Start()
    {
        myself = this.gameObject;
        activeGas = myself.transform.GetChild(0).gameObject;
        gasWarning = myself.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("y"))
		{
            Debug.Log("Firing");
            FireGas();
		}   
    }

    public void FireGas()
	{
        gasWarning.SetActive(true);
        StartCoroutine(waitForTime(startDelay));
        activeGas.SetActive(true);
        gasWarning.SetActive(false);
        StartCoroutine(waitForTime(gasDuration));
        activeGas.SetActive(false);
	}
    IEnumerator waitForTime(float duration)
	{
        yield return new WaitForSeconds(startDelay);
    }
}

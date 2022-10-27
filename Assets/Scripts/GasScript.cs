using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasScript : MonoBehaviour
{
    public float startDelay = 4f;
    public float gasDuration = 8f;
    private GameObject myself;
    private BoxCollider2D collider;
    public ParticleSystem activeGas;
    public int activeEmissionRate = 25;
    public ParticleSystem gasWarning;
    public int warningEmissionRate = 5;
    // Start is called before the first frame updat
    void Start()
    {
        myself = this.gameObject;
        collider = GetComponent<BoxCollider2D>();
        activeGas.emissionRate = 0;
        gasWarning.emissionRate = 0;
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
        //use a coroutine so that I can wait
        StartCoroutine(waitForTime());
	}
    IEnumerator waitForTime()
	{
        //show warning and wait
        gasWarning.emissionRate = warningEmissionRate;
        yield return new WaitForSeconds(startDelay);
        //start damaging gas, stop warning, and wait
        activeGas.emissionRate = activeEmissionRate;
        collider.enabled = true;
        gasWarning.emissionRate = 0;
        yield return new WaitForSeconds(gasDuration);
        //stop damaging gas
        collider.enabled = false;
        activeGas.emissionRate = 0;
    }
}

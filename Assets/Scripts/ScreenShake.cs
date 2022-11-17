using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float duration = 1f;
    public float magnitude = 1f;
    public IEnumerator DoShake (float duration, float magnitude)
	{
        Debug.Log("enum");
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;

        while(elapsed < duration)
		{
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;


            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null; 
		}
        transform.localPosition = originalPos;
	}

    public void Shake()
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    void Start()
    {
        
    }
    void Update()
    {
        /*if(Input.GetKeyDown("o"))
		{
            Debug.Log("Made it");
            StartCoroutine( Shake(duration, magnitude));
		}*/
    }
}

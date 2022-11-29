using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenShake : MonoBehaviour
{
    public float duration = 1f;
    public float magnitude = 1f;
    public IEnumerator DoShake (float duration, float magnitude)
	{
        //Debug.Log("enum");
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0f;
        //Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
        while (elapsed < duration)
		{
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            if (!PauseMenu.GameIsPaused)
            {
                transform.localPosition = new Vector3(x, y, originalPos.z);
            }

            elapsed += Time.deltaTime;

            yield return null; 
		}
        transform.localPosition = originalPos;
        //Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    public void Shake()
    {
        StartCoroutine(DoShake(duration, magnitude));
    }

    public void ShockwaveShake(float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float x = Random.Range(-1f, 1f) * magnitude;
        float y = Random.Range(-1f, 1f) * magnitude;

        if (!PauseMenu.GameIsPaused)
        {
            transform.localPosition = new Vector3(x, y, originalPos.z);
        }
        //transform.localPosition = originalPos;
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

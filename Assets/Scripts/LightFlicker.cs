using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    public float stayOffTime = 0.3f;
    public int numberOfFlickers = 6;
    public float flickerDelay = 0.02f;
    private Light2D m_light;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light2D>();
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Lit", true);
    }

    public void Flicker()
    {
        StartCoroutine(DoFlicker());
    }

    private IEnumerator DoFlicker()
    {
        m_light.intensity = 0;
        animator.SetBool("Lit", false);
        yield return new WaitForSeconds(stayOffTime);
        for (int i = 0; i < numberOfFlickers; i++)
        {
            m_light.intensity = 2;
            animator.SetBool("Lit", true);
            yield return new WaitForSeconds(flickerDelay);
            m_light.intensity = 0;
            animator.SetBool("Lit", false);
            yield return new WaitForSeconds(flickerDelay);
        }
        m_light.intensity = 2;
        animator.SetBool("Lit", true);
    }
}

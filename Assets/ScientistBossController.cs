using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBossController : MonoBehaviour
{
    public float speed;
    public float minimumDistance;
    //public float maximumDistance;
    public Transform target;
    public Transform throwPoint;
    public GameObject flask;
    public LayerMask playerLayer;
    public float throwRate = 0.5f;
    private Animator animator;
    private float nextThrow = 0f;
    private bool jumping = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextThrow)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Reload", true);
        }
        // Move towards the Reaper when he's far enough away
        else if (!animator.GetBool("Reload") && !animator.GetBool("Throw") && !jumping && Vector2.Distance(transform.position, target.position) > minimumDistance + 0.5f)
        {
            animator.SetBool("Moving", true);
            MoveTowards();
        }
        // Move away from the Reaper if he gets too close
        else if (!animator.GetBool("Reload") && !animator.GetBool("Throw") && !jumping && Vector2.Distance(transform.position, target.position) < minimumDistance)
        {
            animator.SetBool("Moving", true);
            MoveAway();
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void MoveAway()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
    }

    // Function for a potential jumping ability
    private void Jump()
    {
        jumping = true;

        jumping = false;
    }

    private void startThrow()
    {
        // Insert throwing animation here, animation will trigger the ThrowFlask() function
        animator.SetBool("Reload", false);
        animator.SetBool("Throw", true);
        nextThrow = Time.time + 1f / throwRate;
    }

    // Function that throws a flask at the Reaper
    private void ThrowFlask()
    {
        Instantiate(flask, throwPoint.position, Quaternion.identity);
        animator.SetBool("Throw", false);
    }

    // Function that allows the scientist to summon poisonous gas
    private void SummonPoisonGas()
    {

    }
}

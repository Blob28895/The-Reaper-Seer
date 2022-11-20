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
    //private Rigidbody2D rb;
    private int health;
    private int maxHealth;
    private bool enableGasAttacks = false;
    private Animator animator;
    private float nextThrow = 0f;
    private float dist;
    private bool facingRight = false;
    private bool jumping = false;

    private Boss boss;
    private float slowMult = 1f;
    private float slowTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boss = GetComponent<Boss>();
        maxHealth = boss.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slowMult = boss.getSlowMult();
        slowTime = boss.getSlowTime();
        //Debug.Log(slowMult);
        if (Time.time >= slowTime && slowMult != 1f)
        {
            slowMult = 1f;
        }
        health = boss.getCurrentHealth();
        // Get the distance to determine which direction the Scientist should be facing
        dist = target.position.x - transform.position.x;
        if (dist > 0 && !facingRight)
        {
            Flip();
        }
        else if (dist < 0 && facingRight)
        {
            Flip();
        }
        // Scientist will enable gas attacks once he reaches below half health
        if (health <= maxHealth / 2 && enableGasAttacks == false)
        {
            enableGasAttacks = true;
            animator.SetBool("Moving", false);
            animator.SetBool("GasButton", true);
        }
        // Allows the scientist to attack. After the reload animation plays, the throw animation will play and the throw animation will
        // trigger the ThrowFlask() function
        else if (Time.time >= nextThrow)
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Reload", true);
        }
        // Move towards the Reaper when he's far enough away
        else if (CanMove() && Vector2.Distance(transform.position, target.position) > minimumDistance + 0.5f)
        {
            animator.SetBool("Moving", true);
            MoveTowards();
        }
        // Move away from the Reaper if he gets too close
        else if (CanMove() && Vector2.Distance(transform.position, target.position) < minimumDistance)
        {
            animator.SetBool("Moving", true);
            MoveAway();
        }
        else
        {
            animator.SetBool("Moving", false);
        }
    }

    private bool CanMove()
    {
        return !animator.GetBool("GasButton") && !animator.GetBool("Reload") && !animator.GetBool("Throw") && !jumping;
    }

    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * slowMult * Time.deltaTime);
    }

    private void MoveAway()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), -speed * slowMult * Time.deltaTime);
    }

    // Function for a potential jumping ability
    private void Jump()
    {
        jumping = true;
        // Code to do the actual jumping goes here
        jumping = false;
    }

    // Function that runs after the reload animation plays
    private void startThrow()
    {
        // Throw animation will trigger the ThrowFlask() function
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
        animator.SetBool("GasButton", false);
        // Call Gas Controller code to start gas attacks
        GetComponent<GasController>().EnableVents();
    }

    // Function that flips the model to face the other direction
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
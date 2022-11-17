using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOSBossController : MonoBehaviour
{
    public float speed;
    // Following are used for melee attacking
    public int attackDamage = 2;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public float attackRate = 1f;
    public LayerMask playerLayer;
    private List<string> meleeAnimations = new List<string>();
    private float nextAttackTime = 0f;

    // Smash attacks
    private ScreenShake shakeObj;
    public float smashAttackIntervalMin = 7f;
    public float smashAttackIntervalMax = 13f;
    public Shockwave shockwaveObject;
    private float nextSmashTime;

    // Call reinforcements
    public float callInterval = 15f;
    private float nextCallTime;

    // Reaper stuff
    public Transform target;
    private float dist;

    // Used for head guard
    private bool facingRight = false;
    private Animator animator;
    private int maxHealth;
    private int health;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        maxHealth = GetComponent<Boss>().maxHealth;
        shakeObj = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();

        // Adds names of melee animations so that we can pick randomly
        meleeAnimations.Add("Melee1");
        meleeAnimations.Add("Melee2");
        meleeAnimations.Add("Melee3");

        // Assign next attack variables here so that hos can't instantly use them upon scene restart
        nextSmashTime = Time.time + 10f;
        nextCallTime = Time.time + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<Boss>().getCurrentHealth();
        dist = target.position.x - transform.position.x;
        if (dist > 0 && !facingRight)
        {
            Flip();
        }
        else if (dist < 0 && facingRight)
        {
            Flip();
        }
        // Make the guard stop moving once his x coordinate matches the Reaper's
        if (Mathf.Abs(dist) <= 0.1)
        {
            animator.SetBool("Moving", false);
        }
        // Move towards the Reaper while he's not in range
        else if (CanMove() && !PlayerInRange())
        {
            animator.SetBool("Moving", true);
            MoveTowards();
        }
        // Head guard will start attacking once the Reaper is in range
        else if (PlayerInRange() && CanMove())
        {
            animator.SetBool("Moving", false);
            if (Time.time >= nextAttackTime)
            {
                Debug.Log("Attacking!");
                int index = Random.Range(0, 3);
                //Attack();
                animator.SetBool(meleeAnimations[index], true);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        // Perform smash attack
        if (Time.time >= nextSmashTime && CanMove())
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Slam", true);
            float smashAttackTime = Random.Range(smashAttackIntervalMin, smashAttackIntervalMax);
            nextSmashTime = Time.time + smashAttackTime;

        }
        // Summon enemies for help
        // At some point, introduce an enemy cap to the if statement so that the hos can't keep summoning new enemies when there are too many on screen
        if (/*health < maxHealth / 2 && */Time.time >= nextCallTime && CanMove())
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Call", true);
            nextCallTime = Time.time + callInterval;
        }
    }

    // Head guard should only be able to move while not performing any kind of attack
    private bool CanMove()
    {
        return !animator.GetBool("Melee1") && !animator.GetBool("Melee2")  && !animator.GetBool("Melee3")
            && !animator.GetBool("Slam") && !animator.GetBool("Call");
    }

    // Detects whether the Reaper is in range or not to allow guard to perform melee attacks
    private bool PlayerInRange()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        return hitPlayer != null;
    }

    // Move towards the Reaper
    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
    }

    // Deal damage to the Reaper if the attack hits him
    private void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }

    // Summon a shockwave to damage the Reaper
    private void Smash()
    {
        Debug.Log("Smash Attack!");
        // Add a major attackRate cooldown
        nextAttackTime = Time.time + 2f;
        // Just don't, lmaooo: GetComponent<ScreenShake>().Shake();
        shakeObj.Shake();
        // Code to spawn shockwave effect prefab goes here
        Shockwave shockwave1 = Instantiate(shockwaveObject, new Vector2(transform.position.x, 0), Quaternion.identity);
        shockwave1.SetDirection(1);
        Shockwave shockwave2 = Instantiate(shockwaveObject, new Vector2(transform.position.x, 0), Quaternion.identity);
        shockwave2.SetDirection(-1);
    }

    // Function that gets called to call reinforcements to fight the Reaper
    private void SummonEnemies()
    {
        Debug.Log("Summoning Reinforcements!");
        // Code to summon enemies (and presumably to temporarily stop the elevator) goes here
    }

    // Called at the end of an attack animation to ensure that head guard can move and use other attacks
    private void ResetBoolVar()
    {
        // Resets all bool variables after performing an attack to allow the head guard to move
        animator.SetBool("Melee1", false);
        animator.SetBool("Melee2", false);
        animator.SetBool("Melee3", false);
        animator.SetBool("Slam", false);
        animator.SetBool("Call", false);
    }

    // Flip the head guard so that he faces the Reaper at all times
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Show attack point in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

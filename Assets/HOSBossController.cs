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
    public float smashAttackRate = 0.1f;
    private float nextSmashTime = 10f;

    // Call reinforcements
    public float callRate = 0.1f;
    private float nextCallRate = 0f;

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

        // Adds names of melee animations so that we can pick randomly
        meleeAnimations.Add("Melee1");
        meleeAnimations.Add("Melee2");
        meleeAnimations.Add("Melee3");
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
        if (CanMove() && !PlayerInRange())
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
        if (Time.time >= nextSmashTime && CanMove())
        {
            animator.SetBool("Moving", false);
            animator.SetBool("ShockwaveAttack", true);
            nextSmashTime = Time.time + 1f / smashAttackRate;

        }
    }

    // Check if the Head guard can move
    private bool CanMove()
    {
        return !animator.GetBool("Melee1") && !animator.GetBool("Melee2")  && !animator.GetBool("Melee3")
            && !animator.GetBool("ShockwaveAttack") && !animator.GetBool("Call");
    }

    // Detects whether the Reaper is in range or not
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
    }

    private void ResetBoolVar()
    {
        // Resets all bool variables after performing an attack
        animator.SetBool("Melee1", false);
        animator.SetBool("Melee2", false);
        animator.SetBool("Melee3", false);
        animator.SetBool("ShockwaveAttack", false);
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

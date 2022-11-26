using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperSeerBossController : MonoBehaviour
{
    public float speed;
    // Following are used for melee attacking
    public int attackDamage = 1;
    public float attackRange = 1f;
    public Transform attackPoint;
    public float attackRate = 1f;
    public LayerMask playerLayer;
    private List<string> meleeAnimations = new List<string>();
    private float nextAttackTime = 0f;
    // Reaper stuff
    public Transform target;
    private float dist;
    // Used for Reaper Seer
    private bool facingRight = false;
    private Animator animator;
    private int maxHealth;
    private int health;
    private Boss boss;
    private float slowMult;
    private float slowTime;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boss = GetComponent<Boss>();
        maxHealth = boss.maxHealth;
        // Adds names of melee animations so that we can pick randomly
        meleeAnimations.Add("Melee1");
        meleeAnimations.Add("Melee2");
    }

    // Update is called once per frame
    void Update()
    {
        slowMult = boss.getSlowMult();
        slowTime = boss.getSlowTime();
        if (Time.time >= slowTime && slowMult != 1f)
        {
            slowMult = 1f;
        }
        health = boss.getCurrentHealth();
        dist = target.position.x - transform.position.x;
        if (dist > 1f && !facingRight)
        {
            Flip();
        }
        else if (dist < -1f && facingRight)
        {
            Flip();
        }
    }

    // Reaper Seer should only be able to move while not performing any kind of attack
    private bool CanMove()
    {
        return true;
    }

    // Detects whether the Reaper is in range or not to allow the Reaper Seer to perform melee attacks
    private bool PlayerInRange()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        return hitPlayer != null;
    }

    // Move towards the Reaper
    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * slowMult * Time.deltaTime);
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

    // Called as an animation event at the end of an attack animation to ensure that the Reaper Seer can move and use other attacks
    private void ResetBoolVar()
    {
        // Resets all bool variables after performing an attack to allow the head guard to move
        animator.SetBool("Melee1", false);
        animator.SetBool("Melee2", false);
        animator.SetBool("Slam", false);

    }

    // Flip the Reaper Seer so that he faces the Reaper at all times
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        // Move Reaper Seer's x coordinate since he's not centered in the sprite
        transform.position += (transform.localScale.x * -1) * new Vector3(0.45f, 0f);
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

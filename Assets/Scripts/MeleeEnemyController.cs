using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    // Used for movement
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float maximumDistance;
    private bool facingRight = true;
    private float dist;
    private Enemy enemy;
    private int currentHealth;
    // Used for attacking
    public Animator animator;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    // Attack rate
    public float attackRate = 2f;
    private float nextAttackTime = 0f;
    public float hitStun = 0.6f;
    private float stunEnd = 0f;
    // Start is called once before update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        currentHealth = enemy.maxHealth;

    }
	public void setStunEnd()
	{
        stunEnd = Time.time + hitStun;
	}
	// Update is called once per frame
	void Update()
    {
        if(enemy.getCurrentHealth() < currentHealth)
		{
            setStunEnd();
		}
        currentHealth = enemy.getCurrentHealth();
        // While the enemy is more than the minimum distance away from the player, the enemy will move toward the player
        if (Vector2.Distance(transform.position, target.position) > minimumDistance && Vector2.Distance(transform.position, target.position) < maximumDistance)
        {
            animator.SetBool("Moving", true);
            Move();
        }

        // When the enemy is close enough to the player, it will start attacking
        else if (Vector2.Distance(transform.position, target.position) <= minimumDistance)
        {
            animator.SetBool("Moving", false);
            // Controls the attack rate
            if (Time.time >= nextAttackTime && Time.time >= stunEnd)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Function that moves the enemy
    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        dist = target.position.x - transform.position.x;
        // Debug.Log(dist);
        if (dist > 0 && !facingRight)
        {
            Flip();
        }
        else if (dist < 0 && facingRight)
        {
            Flip();
        }
    }

    // Function that allows the enemy to attack
    private void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Attack");

        // Detect if the player gets hit
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        //Debug.Log("Player hit " + hitPlayer.name);

        // Add code here to cause the player to take damage
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }

    // Function that flips the model when moving in the other direction
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    // Function so that when we are editing the scene we can visually see the hitbox
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

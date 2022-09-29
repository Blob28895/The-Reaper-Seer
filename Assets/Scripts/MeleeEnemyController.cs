using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : MonoBehaviour
{
    // Used for movement
    public float speed;
    public Transform target;
    public float minimumDistance;

    // Used for attacking
    public Animator animator;
    public Transform attackPoint;
    public LayerMask playerLayer;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    // Attack rate
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    // Update is called once per frame
    void Update()
    {
        // While the enemy is more than the minimum distance away from the player, the enemy will move toward the player
        if (Vector2.Distance(transform.position, target.position) > minimumDistance)
        {
            Move();
        }

        // When the enemy is close enough to the player, it will start attacking
        else
        {
            if (Time.time > nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void Move()
    {
        // Moves the enemy
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Attack");

        // Detect if the player gets hit
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        //Debug.Log("Enemy hit " + hitPlayer.name);

        // Add code here to cause the player to take damage 
    }
}

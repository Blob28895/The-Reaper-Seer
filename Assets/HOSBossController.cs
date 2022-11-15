using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOSBossController : MonoBehaviour
{
    public float speed;
    public int attackDamage = 2;
    public float attackRange = 0.5f;
    public Transform attackPoint;
    public float attackRate = 1f;
    public LayerMask playerLayer;
    private List<string> meleeAnimations = new List<string>();
    private float nextAttackTime = 0f;
    private bool facingRight = false;
    private Animator animator;
    private int maxHealth;
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
        // Head guard will start attacking once the Reaper is in range
        if (PlayerInRange())
        {
            if (Time.time >= nextAttackTime)
            {
                Debug.Log("Attacking!");
                int index = Random.Range(0, 3);
                //Attack();
                animator.SetTrigger(meleeAnimations[index]);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    // Detects whether the Reaper is in range or not
    private bool PlayerInRange()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        return hitPlayer != null;
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

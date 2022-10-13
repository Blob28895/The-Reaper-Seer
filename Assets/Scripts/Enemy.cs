using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public Animator animator;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public int getCurrentHealth()
	{
        return currentHealth;
	}

    public void TakeDamage(int damage)
	{
        if (enabled)
        {
            currentHealth -= damage;
            //Debug.Log("enemy took" + damage + "and is at " + currentHealth + " health");
            //insert hurt animation here
            animator.SetTrigger("Hurt");
            if (currentHealth <= 0)
            {
                Die();
            }
        }
	}

    void Die()
	{
        //Debug.Log("The enemy is dead");
        // Disables the rest of the enemy scripts so that it can't continue attacking the player
        foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
        // Death animation goes here
        animator.SetBool("Dead", true);

        // Disable the enemy
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }
        // GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
	}
}

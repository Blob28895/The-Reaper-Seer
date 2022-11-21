using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    private Animator animator;
    int currentHealth;
    public KillTracker killTracker;

    private float slowMult = 1f;
    private float slowTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        killTracker = GameObject.FindGameObjectWithTag("Player").GetComponent<KillTracker>();
        killTracker.AddNewEnemy(gameObject);
    }
    public int getCurrentHealth()
	{
        return currentHealth;
	}
    public float getSlowMult()
    {
        return slowMult;
    }
    public float getSlowTime()
    {
        return slowTime;
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
    public void Slow()
    {
        slowMult = staticVariables.enemySlowMult;
        slowTime = Time.time + staticVariables.enemySlowTime;
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
        //Count them as dead in the counter
        killTracker.KilledEnemy(gameObject);
        // Disable the enemy
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }
        // GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
        if(SceneManager.GetActiveScene().name == "HOSFight")
		{
            Destroy(gameObject, 1f);
        }
        //
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public static int maxHealth = 10;
    //private int currentHealth;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = staticVariables.currHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Function that applies damage to the player
    public void TakeDamage(int damage)
    {
        if (enabled)
        {
            staticVariables.currHealth -= damage;
            healthBar.SetHealth(staticVariables.currHealth);
            // Insert hurt animation here
            animator.SetTrigger("Hurt");
            //Debug.Log("Player has taken " + damage + " damage. Current health: " + currentHealth);
            if (staticVariables.currHealth <= 0)
            {
                Die();
            }
        }
    }

    // Function that plays the dying animation and disables the player when they run out of health
    private void Die()
    {
        //Debug.Log("Player is now dead.");
        // Makes it so that the player can no longer move after death
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        // Used for disabling both the circle and box collider on the Reaper
        /*
        foreach (Collider2D c in GetComponents<Collider2D>())
        {
            c.enabled = false;
        }
        */
        // Disables the other Reaper scripts so that the player can't continue attacking or performing other actions when they are dead
        foreach (MonoBehaviour script in gameObject.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }
        // Death animation goes here
        animator.SetBool("Dead", true);

        //GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
        // Used for ending the game and giving the player the option to restart
        FindObjectOfType<GameHandler>().GameOver();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth;
    private Animator animator;
    private Renderer renderer;
    int currentHealth;
    public HealthBar healthBar;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(maxHealth);
        }
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
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }
            Debug.Log(currentHealth);
            //Debug.Log("enemy took" + damage + "and is at " + currentHealth + " health");
            //insert hurt animation here
            //animator.SetTrigger("Hurt");
            StartCoroutine(Flash());
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator Flash()
    {
        float duration = Time.time + 0.3f;
        while (Time.time < duration)
        {
            yield return new WaitForSeconds(0.05f);
            renderer.enabled = !renderer.enabled;
        }
        renderer.enabled = true;
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
        //animator.SetBool("Dead", true);
        animator.SetTrigger("Die");
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

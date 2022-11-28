using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    public static int maxHealth = 10;
    //private int currentHealth;
    public HealthBar healthBar;
    public float invicibleTime = 0f;
    public AudioSource takeDamage;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        //currentHealth = staticVariables.currHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(staticVariables.currHealth);
        takeDamage = GetComponentsInChildren<AudioSource>()[1];
        rb = GetComponent<Rigidbody2D>();
    }
    public void FixedUpdate()
    {

    }
    public void invincibility()
    {
        invicibleTime = Time.time + 0.5f;
    }
    public float getInvincibleTime()
    {
        return invicibleTime;
    }
    public void Heal(int heal)
    {
        if (enabled)
        {
            if (staticVariables.currHealth + heal <= maxHealth)
            {
                staticVariables.currHealth += heal;
            }
            else
            {
                staticVariables.currHealth = maxHealth;
            }
            healthBar.SetHealth(staticVariables.currHealth);
        }
    }
    // Function that applies damage to the player
    public void TakeDamage(int damage)
    {
        if (enabled && Time.time >= invicibleTime)
        {
            staticVariables.currHealth -= damage;
            takeDamage.Play();
            healthBar.SetHealth(staticVariables.currHealth);
            if (Gamepad.current != null)
            {
                StartCoroutine(Rumble(0.20f));
            }
            // Insert hurt animation here
            animator.SetTrigger("Hurt");
            //Debug.Log("Player has taken " + damage + " damage. Current health: " + currentHealth);
            if (staticVariables.currHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Knockback(Vector2 force, float time)
    {
        if (enabled)
        {
            StartCoroutine(ApplyKnockback(force, time));
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

    // Coroutine to rumble the controller when getting hit
    private IEnumerator Rumble(float duration)
    {
        Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
        yield return new WaitForSeconds(duration);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }

    private IEnumerator ApplyKnockback(Vector2 force, float time)
    {
        float knockBackDuration = Time.time + time;
        while (Time.time <= knockBackDuration)
        {
            rb.velocity = force;
            yield return null;
        }
        rb.velocity = new Vector2(force.x, 0);
    }
}

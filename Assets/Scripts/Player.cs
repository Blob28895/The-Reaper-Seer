using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function that applies damage to the player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Insert hurt animation here

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function that plays the dying animation and disables the player when they run out of health
    private void Die()
    {
        // Death animation goes here

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public int attackDamage = 1;
    public float speed;
    public int spinSpeed = 135;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 reaperPosition = FindObjectOfType<CharacterController2D>().transform.position;
        targetPosition.Set(reaperPosition.x, reaperPosition.y + 8f, 0);
        rb.AddForce((targetPosition - transform.position) * speed);
        // Change the spin direction depending on which direction the flask is thrown
        if (targetPosition.x - transform.position.x < 0)
        {
            spinSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
    }

    // Function that runs when the flask collides with something
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Do not collide with the scientist
        if (collision.name != "Scientist")
        {
            //Debug.Log(collision.name);
            // If the flask hits the Reaper, deal damage
            if (collision.name == "Reaper")
            {
                collision.GetComponent<Player>().TakeDamage(attackDamage);
            }
            // Remove the flask object after colliding
            Destroy(gameObject);
        }
    }
}

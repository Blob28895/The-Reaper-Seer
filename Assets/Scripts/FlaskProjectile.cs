using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public int attackDamage = 1;
    public float speed;
    public int spinSpeed = 135;
    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        Debug.Log(collision.name);
        // Do not collide with the scientist, gas, or the platform
        if (collision.name != "Scientist" && enabled && collision.name != "Platform" && collision.gameObject.tag != "Vent")
        {
            //Debug.Log(collision.name);
            enabled = false;
            // If the flask hits the Reaper, deal damage
            if (collision.name == "Reaper")
            {
                collision.GetComponent<Player>().TakeDamage(attackDamage);
            }
            // Remove the flask object after colliding
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetTrigger("Break");
            // float time = animator.GetCurrentAnimatorClipInfo(0).Length;
            Destroy(gameObject, 0.75f);
        }
    }
}

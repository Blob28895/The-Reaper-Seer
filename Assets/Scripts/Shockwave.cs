using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public int attackDamage = 1;
    public float speed;
    private int direction;
    private float nextAttackTime = 0f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(direction, transform.localScale.y);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(speed * direction, 0));
    }

    // Sets the direction the shockwave should face and move towards
    public void SetDirection(int d)
    {
        direction = d;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        // Deal damage to the Reaper
        if (collision.name == "Reaper" && Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + 0.5f;
            collision.GetComponent<Player>().TakeDamage(attackDamage);
        }
        // Destroy shockwave object after hitting the elevator wall
        if (collision.name == "InvisWall" || collision.name == "InvisWall (1)")
        {
            Destroy(gameObject);
        }
    }
}

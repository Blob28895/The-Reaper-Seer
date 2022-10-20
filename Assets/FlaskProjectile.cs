using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskProjectile : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed;
    public int spinSpeed = 90;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 reaperPosition = FindObjectOfType<CharacterController2D>().transform.position;
        targetPosition.Set(reaperPosition.x, reaperPosition.y + 8f, 0);
        rb.AddForce((targetPosition - transform.position) * speed);
        //targetPosition = reaperPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));
        //transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Scientist")
        {
            Debug.Log(collision.name);
            // Breaking animation here

            Destroy(gameObject);
        }
    }
}

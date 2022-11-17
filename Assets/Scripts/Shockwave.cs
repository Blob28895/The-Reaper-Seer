using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shockwave : MonoBehaviour
{
    public int attackDamage = 1;
    public float speed;
    private int direction;
    private float nextAttackTime = 0f;
    private Rigidbody2D rb;
    private float leftInitRumble;
    private float rightInitRumble;
    private GameObject targetWall;
    private float wallPosition;
    private float totalDistance;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(direction, transform.localScale.y);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(speed * direction, 0));
        leftInitRumble = 0.25f;
        rightInitRumble = 0.75f;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(GameObject wall in walls)
        {
            if (transform.position.x - wall.GetComponent<Transform>().position.x > 0 && direction == -1)
            {
                targetWall = wall;
                break;
            }
            else if (transform.position.x - wall.GetComponent<Transform>().position.x < 0 && direction == 1)
            {
                targetWall = wall;
                break;
            }
        }
        wallPosition = targetWall.GetComponent<Transform>().position.x;
        totalDistance = Mathf.Abs(transform.position.x - wallPosition);
        Debug.Log(totalDistance);
    }

    void Update()
    {
        if (targetWall != null)
        {
            float distancePercent = Mathf.Abs(transform.position.x - wallPosition) / totalDistance;
            Debug.Log(distancePercent);
            float leftRumble = leftInitRumble * distancePercent;
            float rightRumble = rightInitRumble * distancePercent;
            if (!PauseMenu.GameIsPaused)
            {
                Gamepad.current.SetMotorSpeeds(leftRumble, rightRumble);
            }
        } 
    }

    // Sets the direction the shockwave should face and move towards
    public void SetDirection(int d)
    {
        direction = d;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        // Deal damage to the Reaper
        if (collision.name == "Reaper" && Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + 0.5f;
            collision.GetComponent<Player>().TakeDamage(attackDamage);
        }
        // Destroy shockwave object after hitting the elevator wall
        if (collision.name == "InvisWall" || collision.name == "InvisWall (1)")
        {
            Gamepad.current.SetMotorSpeeds(0f, 0f);
            Destroy(gameObject);
        }
    }
}

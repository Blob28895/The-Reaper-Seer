using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRoundU : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D bulletrb;

    private float bullettimer;

    public float flytime;
    public float force;
    public int attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        bulletrb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");


        Vector3 direction = player.transform.position - transform.position;
        bulletrb.velocity = new Vector2(direction.x, (direction.y) * 7).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
    }

    // Update is called once per frame
    void Update()
    {
        bullettimer += Time.deltaTime;

        if (bullettimer > flytime)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}

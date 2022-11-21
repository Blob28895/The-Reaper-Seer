using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasDamage : MonoBehaviour
{

	public float hitRate = 1.5f;
	private float nextHitTime = 0f;
	private Player player;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log(collision.name);
		if (collision.name == "Reaper")
		{
			
			player = collision.gameObject.GetComponent<Player>();
			if (Time.time >= nextHitTime && Time.time >= player.getInvincibleTime())
			{
				player.TakeDamage(1);
				nextHitTime = Time.time + hitRate;
			}

		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.name == "Reaper")
		{

			player = collision.gameObject.GetComponent<Player>();
			if (Time.time >= nextHitTime && Time.time >= player.getInvincibleTime())
			{
				player.TakeDamage(1);
				nextHitTime = Time.time + hitRate;
			}

		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

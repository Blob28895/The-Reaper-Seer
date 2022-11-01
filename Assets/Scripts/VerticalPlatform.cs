using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    public float waitTime = 0f;
    private bool colliding = false;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.name == "Reaper")
		{
            colliding = true;
		}
	}
	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.name == "Reaper")
		{
			colliding = true;
		}
	}
	private void OnCollisionExit2D(Collision2D collision)
	{
		if(collision.gameObject.name == "Reaper")
		{
			colliding = false;
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        effector = GetComponent<PlatformEffector2D>();    
    }

    // Update is called once per frame
    void Update()
    {
		/*if (Input.GetKeyUp("s"))
		{
            waitTime = 0.5f;
		}*/
		Debug.Log(colliding);
        if(Input.GetKey("s") && colliding)
		{
            /*if(waitTime <=0)
			{*/
                effector.rotationalOffset = 180f;
                //waitTime = 0.5f;

			/*}
            else
			{
                waitTime -= Time.deltaTime;
			}*/
		}

        if(Input.GetKey("w") || Input.GetKey(KeyCode.Space))
		{
            effector.rotationalOffset = 0;
		}
    }
}

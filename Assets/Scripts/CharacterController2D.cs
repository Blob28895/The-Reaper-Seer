using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask m_GroundNoPlatform;
	[SerializeField] private LayerMask m_WhatisEnemy;
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;// A collider that will be disabled when crouching
	[SerializeField] private BoxCollider2D BoxCollider;
	[SerializeField] private CircleCollider2D CircleCollider;
	[SerializeField] private float m_dashDistance = 3f;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private Vector3 targetVelocity;
	private Player player;
	//public RaycastHit2D hit;

	// Variables for dash effect
	private Vector3 beforeDash;
	[SerializeField] private GameObject reaperDashEffect;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		player = GetComponent<Player>();
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}
	public Vector3 getVelocity()
	{
		return targetVelocity;
	}
	public Rigidbody2D getRB()
	{
		return m_Rigidbody2D;
	}
	public bool getFacing()
	{
		return m_FacingRight;
	}
	public bool getGrounded()
	{
		return m_Grounded;
	}
	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		float extraHeight = 0.5f;
		// Using a boxcast to determine if the Reaper is on the ground
		RaycastHit2D raycastHit = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0f, Vector2.down, extraHeight, m_WhatIsGround);

		// The following lines until m_Grounded is only for debugging purposes
		Color rayColor;
		if (raycastHit.collider != null)
		{
			rayColor = Color.green;
		}
		else
		{
			rayColor = Color.red;
		}
		Debug.DrawRay(BoxCollider.bounds.center + new Vector3(BoxCollider.bounds.extents.x, 0), Vector2.down * (BoxCollider.bounds.extents.y + extraHeight), rayColor);
		Debug.DrawRay(BoxCollider.bounds.center - new Vector3(BoxCollider.bounds.extents.x, 0), Vector2.down * (BoxCollider.bounds.extents.y + extraHeight), rayColor);
		Debug.DrawRay(BoxCollider.bounds.center - new Vector3(BoxCollider.bounds.extents.x, BoxCollider.bounds.extents.y + extraHeight), Vector2.right * (BoxCollider.bounds.extents.x * 2), rayColor);
		//Debug.Log(raycastHit.collider);

		// Considered grounded if it hits any collider that is on the m_WhatIsGround Layermask
		m_Grounded = raycastHit.collider != null;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				//m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public void dodge(Vector3 direction)
	{
		beforeDash = transform.position;
		RaycastHit2D[] hitsTop;
		RaycastHit2D[] hitsBottom;
		RaycastHit2D[] hitsMiddle;
		Vector2 temp = transform.position;
		// Right
		if(direction.x > 0) // get neccessary offset of the hitboxes
		{
			temp.x += (BoxCollider.size.x / 2) + BoxCollider.offset.x;
		}
		// Left
		if(direction.x < 0)
		{
			temp.x -= (BoxCollider.size.x / 2) + BoxCollider.offset.x; 
		}
		// Up
		if(direction.y > 0)
		{
			temp.y += (BoxCollider.size.y / 2) + BoxCollider.offset.y;
		}
		// Down
		if(direction.y < 0)
		{
			temp.y -= (CircleCollider.radius) - BoxCollider.offset.y;
		}
		direction *= m_dashDistance;
		Vector2 topRay = temp;
		Vector2 bottomRay = temp;
		topRay.y = m_CeilingCheck.position.y - ((GetComponent<Renderer>().bounds.size.y) / 10);
		bottomRay.y = m_GroundCheck.position.y + ((GetComponent<Renderer>().bounds.size.y) / 10);
		hitsTop = Physics2D.RaycastAll(topRay, direction, m_dashDistance, m_GroundNoPlatform);
		hitsBottom = Physics2D.RaycastAll(bottomRay, direction, m_dashDistance, m_GroundNoPlatform);
		hitsMiddle = Physics2D.RaycastAll(m_Rigidbody2D.position, direction, m_dashDistance, m_GroundNoPlatform);
		if(staticVariables.dashDamage)
		{
			List<Enemy> enemyObjects = new List<Enemy>();
			RaycastHit2D[] hitsEnemy;
			hitsEnemy = Physics2D.RaycastAll(m_Rigidbody2D.position, direction, m_dashDistance, m_WhatisEnemy);
			foreach(RaycastHit2D enemy in hitsEnemy)
			{
				if(!enemyObjects.Contains(enemy.collider.gameObject.GetComponent<Enemy>()) && enemy.collider.gameObject.GetComponent<Enemy>() != null)
				{
					enemyObjects.Add(enemy.collider.gameObject.GetComponent<Enemy>());
				}
				else if(enemy.collider.gameObject.GetComponent<Boss>() != null)
				{
					enemy.collider.gameObject.GetComponent<Boss>().TakeDamage((int)(staticVariables.baseDamage * staticVariables.damageMultiplier * 0.5f));
					enemy.collider.gameObject.GetComponent<Boss>().Slow();
				}
			}
			foreach(Enemy enemy in enemyObjects)
			{
				enemy.TakeDamage((int) (staticVariables.baseDamage * staticVariables.damageMultiplier * 0.5f));
				enemy.Slow();
			}
		}
		temp.y -= CircleCollider.offset.y;
		//Debug.DrawRay(topRay, direction, Color.red, 1.5f);//hitbox check test ray
		//Debug.DrawRay(bottomRay, direction, Color.red, 1.5f);
		//Debug.DrawRay(transform.position, direction, Color.green, 1.5f); //movement check test ray
		if (hitsTop.Length == 0 && hitsBottom.Length == 0 && hitsMiddle.Length == 0) // if there is nothing blocking our dash
		{
			//Debug.Log("dash"); test if we made it into this if statement
			m_Rigidbody2D.transform.position += direction;
			DashEffect(direction);
		}
		else if (hitsTop.Length > 0 || hitsBottom.Length > 0 || hitsMiddle.Length > 0)
		{ //if there is something blocking our dash we still want to move as far as we can without going through walls
		  //figure out which one hit something
			RaycastHit2D[] hitRay;

			/*if (hitsTop.Length != 0 && hitsBottom.Length == 0 && hitsMiddle.Length == 0)
			{
				hitRay = hitsTop;
			}
			else if(hitsBottom.Length != 0 && hitsTop.Length == 0 && hitsMiddle.Length == 0)
			{
				hitRay = hitsBottom;
			}
			else if(hitsBottom.Length == 0 && hitsTop.Length == 0 && hitsMiddle.Length != 0)
			{
				hitRay = hitsMiddle;
			}
			else
			{
				if (direction.y > 0 && hitsTop.Length != 0) { hitRay = hitsTop; }
				else if (direction.y < 0 && hitsBottom.Length != 0) { hitRay = hitsBottom; }
				else { hitRay = hitsMiddle; }
			}*/

			
			Vector2 failedDash = (m_Rigidbody2D.transform.position + direction);
			float minDist = 1000f;
			if (hitsBottom.Length != 0 && Vector2.Distance(hitsBottom[0].point, failedDash) < minDist){
				minDist = Vector2.Distance(hitsBottom[0].point, failedDash);
			}
			if (hitsMiddle.Length != 0 && Vector2.Distance(hitsMiddle[0].point, failedDash) < minDist)
			{
				minDist = Vector2.Distance(hitsMiddle[0].point, failedDash);
			}
			if (hitsTop.Length != 0 && Vector2.Distance(hitsTop[0].point, failedDash) < minDist)
			{
				minDist = Vector2.Distance(hitsTop[0].point, failedDash);
			}


			//Vector2 contactPoint = hitRay[0].point;
			//float failDistance = Vector2.Distance(contactPoint, failedDash);
			direction /= m_dashDistance;
			Vector3 spriteSize = new Vector3(0,0,0);
			// Right
			if (direction.x > 0) // get neccessary offset of the hitboxes
			{
				spriteSize.x += BoxCollider.size.x*2;
			}
			// Left
			if (direction.x < 0)
			{
				spriteSize.x -= BoxCollider.size.x*2;
			}
			// Up
			if (direction.y > 0)
			{
				spriteSize.y += GetComponent<Renderer>().bounds.size.y;
			}
			// Down
			if (direction.y < 0)
			{
				spriteSize.y -= GetComponent<Renderer>().bounds.size.y;
			}
			// Downward diagonal (left or right)
			if(direction.x != 0 && direction.y < 0) //if diagonal
			{
				spriteSize.y /= 2;
			}
			// Upward diagonal (left or right)
			if(direction.x != 0 && direction.y > 0)
			{
				spriteSize /= 2;
			}
			// ???
			if(direction.x == 0 && direction.y > 0)
			{
				spriteSize.y /= 2;
			}
			m_Rigidbody2D.transform.position += (direction * (m_dashDistance - /*failDistance*/minDist)) - spriteSize/2;
			// Only play the dash effect if the Reaper actually moves when dashing
			if ((m_Rigidbody2D.transform.position - beforeDash).magnitude > 0.1f)
			{
				player.invincibility();
				DashEffect(direction);
			}
		}
		
	}

	private void DashEffect(Vector3 direction)
    {
		int sign = 1;
		if (direction.y < 0)
        {
			sign = -1;
        }
		float angle = Vector3.Angle(Vector3.right, direction) * sign; // Calculate the angle the dash effect should follow with respect to the right direction
		//Debug.Log(angle);
		GameObject dashObject = Instantiate(reaperDashEffect, beforeDash + direction / 1.75f, Quaternion.identity); // Clone the effect at the location the Reaper was last standing
		dashObject.transform.eulerAngles = new Vector3(0, 0, angle); // Apply the angle
		float actualDashDistance = (m_Rigidbody2D.transform.position - beforeDash).magnitude;
		//Debug.Log(actualDashDistance);
		dashObject.transform.localScale = new Vector3(actualDashDistance / 1.5f, 1f, 1f); // Stretch the effect depending on dash distance
		Destroy(dashObject, 0.25f); // Destroy the clone after the animation plays GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

	

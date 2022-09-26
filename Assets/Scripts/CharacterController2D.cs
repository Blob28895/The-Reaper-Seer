using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
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
	//public RaycastHit2D hit;

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

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}
	public Vector3 getVelocity()
	{
		return targetVelocity;
	}
	public bool getFacing()
	{
		return m_FacingRight;
	}
	public bool getGrounded()
	{
		return m_Grounded;
	}
	public Transform getFrontEdge()
	{
		Vector2 output = BoxCollider.offset;
		Debug.Log(output.x);
		float edge = BoxCollider.size.x / 2;
		
		return BoxCollider.transform;
	}
	public Transform getTopEdge()
	{
		return BoxCollider.transform;
	}
	public Transform getBottomEdge()
	{
		return BoxCollider.transform;
	}
	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
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

		//direction *= 0.05f;

		/*RaycastHit2D[] hits;
		hits = Physics2D.RaycastAll(transform.position, direction, m_dashDistance, m_WhatIsGround);
		Debug.DrawRay(transform.position, direction, Color.green);
		for (int i = 0; i < hits.Length; i++)
		{
			
			RaycastHit2D hit = hits[i];
			Renderer rend = hit.transform.GetComponent<Renderer>();
			Debug.Log(hit.collider.gameObject.name);
		}*/
			
		//m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, direction, ref m_Velocity, m_MovementSmoothing);
		RaycastHit2D[] hits;
		Vector2 temp = transform.position;
		if(direction.x > 0)
		{
			temp.x += (BoxCollider.size.x / 2) + BoxCollider.offset.x;
		}
		if(direction.x < 0)
		{
			temp.x -= (BoxCollider.size.x / 2) + BoxCollider.offset.x; 
		}
		if(direction.y > 0)
		{
			temp.y += (BoxCollider.size.y / 2) + BoxCollider.offset.y;
		}
		if(direction.y < 0)
		{
			temp.y -= (CircleCollider.radius) - BoxCollider.offset.y;
		}
		direction *= m_dashDistance;
		//float bugFix = m_dashDistance * 0.05f;
		//Debug.Log(transform.position.x + "|" + transform.position.y + "_-_" + temp.x + "|" + temp.y);
		Debug.Log(direction.x + "|" + direction.y);
		hits = Physics2D.RaycastAll(temp, direction, m_dashDistance, m_WhatIsGround);
		temp.y -= CircleCollider.offset.y;
		Debug.DrawRay(temp, direction, Color.red, 1.5f);
		Debug.DrawRay(transform.position, direction, Color.green, 1.5f);
		if (hits.Length == 0)
		{

			//Debug.DrawRay(transform.position, direction, Color.green);
			Debug.Log("dash");
			m_Rigidbody2D.transform.position += direction;
			
			;
		}
		else
		{
			Vector2 failedDash = (m_Rigidbody2D.transform.position + direction);
			Vector2 contactPoint = hits[0].point;
			float distance = Vector2.Distance(contactPoint, failedDash);
			direction /= m_dashDistance;
			Vector3 spriteSize = new Vector3(0,0,0);
			if (direction.x > 0)
			{
				spriteSize.x += BoxCollider.size.x;
			}
			if (direction.x < 0)
			{
				spriteSize.x -= BoxCollider.size.x;
			}
			if (direction.y > 0)
			{
				spriteSize.y += GetComponent<Renderer>().bounds.size.y;
			}
			if (direction.y < 0)
			{
				spriteSize.y -= GetComponent<Renderer>().bounds.size.y;
			}
			m_Rigidbody2D.transform.position += (direction * (m_dashDistance - distance)) - spriteSize/2;
			/*for (int i = 0; i < hits.Length; i++)
			{

				RaycastHit2D hit = hits[i];
				Renderer rend = hit.transform.GetComponent<Renderer>();
				Debug.Log(hit.collider.gameObject.name);
			}*/
			
			//Debug.Log("fuck you");
		}
		
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

	

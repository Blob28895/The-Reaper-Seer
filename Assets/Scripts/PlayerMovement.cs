using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this line is being added as a test
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D RB;
    // When you create public variables up here they are adjustable in the unity UI
    public BoxCollider2D BoxCollider;
    public CircleCollider2D CircleCollider;
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    private float xmove = 0f;
    private float ymove = 0f;
    private bool facingRight = true;
    private bool isDodging = false;
    private Vector3 moveDir = new Vector3(0, 0, 0);
    private bool isAttacking = false;
    public float attackSlowMultiplier;

    private bool airDodged = false;
    float nextDodgeTime = 0f;
    public float DodgeCooldown = 0.6f;
    public void setAttacking(bool set)
    {
        isAttacking = set;
    }
	void Start()
	{
        RB = controller.getRB();
	}
	// Update is called once per frame
	// Use this to get input
	void Update()
    {
        if(!isDodging) { 
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed * staticVariables.movementMultiplier;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        animator.SetBool("grounded", controller.getGrounded());
        animator.SetFloat("Vertical", RB.velocity.y);
        
        if(Input.GetButtonDown("Jump"))
		{
            jump = true;
		}
            if (Input.GetButtonDown("Dodge") && Time.time >= nextDodgeTime && airDodged == false)
            {
                Vector3 currVelocity = controller.getVelocity(); // returns the current velocity of the character
                nextDodgeTime = Time.time + DodgeCooldown;
                facingRight = controller.getFacing();
                if (Input.GetKey("d")) //figuring out which direction to dash
                {
                    xmove += 1.0f;
                }
                if (Input.GetKey("a"))
                {
                    xmove += -1.0f;
                }
                if (Input.GetKey("w"))
                {
                    ymove += 1.0f;
                }
                if (Input.GetKey("s"))
                {
                    ymove += -1.0f;
                }
                if (xmove == 0f && ymove == 0f) //if there is no direction, but the dodge button was pressed, we still want to dodge
				{
                    if(facingRight) // dodge the direction we are facing
					{
                        xmove = 1.0f;
					}
					else
					{
                        xmove = -1.0f;
					}
				}
                isDodging = true;
                //Debug.Log(xmove + "|" + ymove);
                if(controller.getGrounded() == false)
				{
                    airDodged = true;
				}
                moveDir = new Vector3(xmove, ymove, 0).normalized;
            }
        }
        if(controller.getGrounded() == true)
		{
            airDodged = false;
		}
    }

    //This function is dedicated for physics
	void FixedUpdate()
	{
        //      With this specific character controller the "false, false" means "i do not want to crouch, and I do not want to jump"
        if (!isDodging)
        {
            if (isAttacking) {
                horizontalMove *= attackSlowMultiplier;
            }
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump); // Time.fixedDeltaTime is the amount of time that has passed since the last time FixedUpdate() was called
            jump = false;
        }
        if(isDodging)
		{
            controller.dodge(moveDir);
            isDodging = false;
            moveDir = new Vector3(0, 0, 0);
            xmove = 0f;
            ymove = 0f;
		}

	}
}

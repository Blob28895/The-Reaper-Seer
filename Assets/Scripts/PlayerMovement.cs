using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this line is being added as a test
public class PlayerMovement : MonoBehaviour
{
    private enum State
	{
        Normal,
        Rolling
	}
    // When you create public variables up here they are adjustable in the unity UI
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    private State state = State.Normal;
    private float xmove = 0f;
    private float ymove = 0f;
    private bool facingRight = true;
    // Update is called once per frame
    // Use this to get input
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if(Input.GetButtonDown("Jump"))
		{
            jump = true;
		}
        if(Input.GetButtonDown("Dodge"))
		{
            Vector3 currVelocity = controller.getVelocity(); // returns the current velocity of the character
            //These if statements take the velocity and turn it into basically boolean values of 1 if moving in that direction and 0 if not
            facingRight = controller.getFacing();
            if (Input.GetKey("d") )
			{
                xmove = 1.0f;
			}
            if(Input.GetKey("a"))
			{
                xmove = -1.0f;
			}
            if(Input.GetKey("a") && Input.GetKey("d"))
			{
                xmove = 0f;
			}
            if (Input.GetKey("w"))
            {
                ymove = 1.0f;
            }
            if (Input.GetKey("s"))
            {
                ymove = -1.0f;
            }
            if (Input.GetKey("w") && Input.GetKey("s"))
            {
                ymove = 0f;
            }

            Debug.Log(xmove + "|" + ymove);
            /*xmove = 0f;
            ymove = 0f;
             */
        }
    }

    //This function is dedicated for physics
	void FixedUpdate()
	{
        //      With this specific character controller the "false, false" means "i do not want to crouch, and I do not want to jump"
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
        // Time.fixedDeltaTime is the amount of time that has passed since the last time FixedUpdate() was called


	}
}

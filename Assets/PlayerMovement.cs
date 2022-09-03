using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // When you create public variables up here they are adjustable in the unity UI
    public CharacterController2D controller;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    // Update is called once per frame
    // Use this to get input
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if(Input.GetButtonDown("Jump"))
		{
            jump = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
	public Animator animator;
	public Transform attackPoint;
	public float attackRange = 0.5f;
	public LayerMask enemyLayers;
	public int attackDamage = 25;
	public CharacterController2D controller;
	public PlayerMovement movement;
	//attackRate is attacks per second
	public float attackRate = 2f;
	public float attackDelay = 1f;
	public float comboTransitionDelay = 0.25f;
	public float secondTransitionDelay = 0.35f;
	float nextAttackTime = 0f;
	bool inCombo = false;
	float comboTime = 0f;
	//animation times
	float attack1Time = 0f;
	float attack2Time = 0f;
	float attack3Time = 0f;
	float transition1Time = 0f;
	float transition2Time = 0f;

	void Start()
	{
		UpdateAnimClipTimes();
	}
	public void UpdateAnimClipTimes()
	{
		AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
		foreach (AnimationClip clip in clips)
		{
			switch (clip.name)
			{
				case "attack1":
					attack1Time = clip.length;
					break;
				case "attack2":
					attack2Time = clip.length;
					break;
				case "attack3":
					attack3Time = clip.length;
					break;
				case "transition1":
					transition1Time = clip.length;
					break;
				case "transition2":
					transition2Time = clip.length;
					break;

			}
		}
	}
	void Update()
	{
		//Debug.Log(transition1Time);
		
		string state = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
		//Debug.Log(state);
		/*transition1Time = comboTransitionDelay;
		transition2Time = secondTransitionDelay;*/
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetButtonDown("Fire3")) // if we push the fire button
			{

				/*This is a Multiline comment to kind of explain this code to both myself, and whomever may come across this mess
				 
				Next Attack Time: A time in the form of how much time has passed since the scene started, we only even check whether or not the
				attack button is pressed if we are at or past the next Attack Time

				State: just the name of the animation that is currently playing

				comboTime: when this timer runs out it means the combo is over

				inCombo: we only want to end the combo when we are actually in a combo to begin with, so thats why this is here

				Attack(): the function handles many things, it decides which animation needs to play, plays it, then applies damage to things

				attackDelay: this is the time between combos

				The first if statement is just when we arent attacking, to be honest I could probably replace all the state checks and just check if we are
				inCombo, but this seems to work

				The else if statement is the check to see if we are doing the third attack this is checked before the 2nd attack check because i needed
				the second attack check to be an else statement, so it needed to be at the end

				The else statement is for the second attack check. To be honest, at the time of writing this I dont fully understand why I need this to be an else
				statement and not an if else like for the second attack, but I tried this and it seems to be working
				 */
				if (state != "attack1" && state != "attack2" && state != "attack3" && state != "transition1" && state != "transition2")
				{ //if we arent alredy attacking
					Attack();
					nextAttackTime = Time.time + attack1Time;
					inCombo = true;
				//comboTime = Time.time + attack1Time + transition1Time;
				comboTime = Time.time + attack1Time + comboTransitionDelay;
				//Debug.Log("Attack 1");
				//Debug.Log(attack1Time);
					//Debug.Log(comboTime - nextAttackTime);
				}
				else if (state == "transtion2" || state == "attack2")
				{
					Attack();
					nextAttackTime = Time.time + attack3Time + attackDelay;
					inCombo = false;
					//Debug.Log("Attack 3");
				}
				else /*if (state == "transition1"*/ /*|| state == "attack1")*/
				{
					Attack();
					nextAttackTime = Time.time + attack2Time;
					//Debug.Log(nextAttackTime);
					inCombo = true;
					//comboTime = Time.time + attack2Time + transition2Time;
					comboTime = Time.time + attack2Time + comboTransitionDelay;
					//Debug.Log("Attack 2");
					//Debug.Log(attack2Time);
					//Debug.Log(comboTime - nextAttackTime);
				
				}
			}
		}
		if (inCombo && Time.time >= comboTime)
		{
			//Debug.Log(Time.time);
			animator.SetTrigger("endCombo");
			inCombo = false;
			nextAttackTime = Time.time + attackDelay;
		}

		if (state == "attack1" || state == "attack2" || state == "attack3" || state == "transition1" || state == "transition2")
		{
			//Debug.Log("attacking");

			movement.setAttacking(true);
		}
		else
		{
			movement.setAttacking(false);
		}
	}
	void Attack()
	{
		//play attack animation
		string state = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
		//Debug.Log(state);
		//if we are not already attacking we can set the attack1 trigger
		if (state != "attack1" && state != "attack2" && state != "attack3" && state != "transition1" && state != "transition2")
		{
			animator.SetTrigger("attack1");
		}
		else if (state == "transition1" || state == "attack1") //if we are in transition then play attack 2
		{
			//Debug.Log(transition1Time);
			animator.SetTrigger("attack2");
		}
		else if (state == "transition2" || state == "attack2") // if we are in transition2 then play attack 3
		{
			animator.SetTrigger("attack3");
		}

		//Detect enemies in range of attack
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		//Apply damage to those enemies
		foreach (Collider2D enemy in hitEnemies)
		{
			Debug.Log("We hit " + enemy.name);
			enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
		}
	}

	//function so that when we are editing the scene we can visually see the hitbox
	void OnDrawGizmosSelected()
	{
		if (attackPoint == null)
		{
			return;
		}
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}

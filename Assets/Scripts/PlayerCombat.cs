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
		transition1Time = comboTransitionDelay;
		transition2Time = secondTransitionDelay;
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetButtonDown("Fire3")) // if we push the fire button
			{
				if (state != "attack1" && state != "attack2" && state != "attack3" && state != "transition1" && state != "transition2")
				{ //if we arent alredy attacking
					Attack();
					nextAttackTime = Time.time + attack1Time;
					inCombo = true;
					comboTime = Time.time + attack1Time + transition1Time;
					//Debug.Log("Attack 1");
				}
				else if (state == "transition1" || state == "attack1")
				{
					Attack();
					nextAttackTime = Time.time + attack2Time;
					//Debug.Log(nextAttackTime);
					inCombo = true;
					comboTime = Time.time + attack2Time + transition2Time;
					//Debug.Log("Attack 2");
				}
				else if (state == "transtion2" || state == "attack2")
				{
					Attack();
					nextAttackTime = Time.time + attack3Time + attackDelay;
					inCombo = false;
					//Debug.Log("Attack 3");
				}
			}
		}
		if (inCombo && Time.time >= comboTime)
		{
			//Debug.Log(Time.time);
			animator.SetTrigger("endCombo");
			inCombo = false;
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

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
	public PlayerMovement movement;
	//attackRate is attacks per second
	public float attackRate = 2f;
	float nextAttackTime = 0f;

	void Start()
	{
		movement = GetComponent<PlayerMovement>();
	}
	// Update is called once per frame
	void Update()
    {
		string state = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
		if (Time.time >= nextAttackTime)
		{
			if (Input.GetButtonDown("Fire3"))
			{
				Attack();
				nextAttackTime = Time.time + 1f / attackRate; 
			}
		}
		if (state == "attack1" || state == "attack2" || state == "attack3" || state == "transition1" || state == "transition2" || state == "Attack")
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
		animator.SetTrigger("Attack");
		//Detect enemies in range of attack
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
		//Apply damage to those enemies
		foreach(Collider2D enemy in hitEnemies)
		{
			//Debug.Log("We hit " + enemy.name);
			enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
		}
	}

	//function so that when we are editing the scene we can visually see the hitbox
	void OnDrawGizmosSelected()
	{
		if(attackPoint == null)
		{
			return;
		}
		Gizmos.DrawWireSphere(attackPoint.position, attackRange);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticVariables : MonoBehaviour
{
	//default stats from other scripts
    public static int currHealth = Player.maxHealth;
	public static int baseDamage = PlayerCombat.attackDamage;
	//upgrades
	public static float damageMultiplier = 1f;
	public static int extraHealth = 0;
	public static bool dashDamage = true;
	public static float movementMultiplier = 1f;

	public void Start()
	{
		
	}
	void Update()
	{
	}
}

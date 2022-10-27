using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class staticVariables : MonoBehaviour
{
    public static int currHealth = Player.maxHealth;

	public void Start()
	{
		/*if(currHealth == 0)
		{
			currHealth = Player.maxHealth;
		}*/
	}
	void Update()
	{
		Debug.Log(currHealth);
	}
}

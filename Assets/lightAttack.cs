using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAttack : MonoBehaviour
{
    private Animator animator;

    public float attackBetween = 0.75f;
    public float comboDelay = 1f;
    private float nextCombo = 0f;
    private int attackTracker = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         if(Input.GetButtonDown("altAttack") && Time.time >= nextCombo)
		{
            if(attackTracker == 0)
			{
                attackTracker++;
			}
            else if (attackTracker == 1)
			{
                attackTracker++;
			}
            else if (attackTracker == 2)
			{
                attackTracker = 0;
			}
		}    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightAttack : MonoBehaviour
{
    private Animator animator;

    public float attackBetween = 0.75f;
    public float breakCombo = 0f;
    public float comboDelay = 1f;
    private float nextCombo = 0f;
    private int attackTracker = 0;

    public GameObject streak1;
    public GameObject streak2;
    public GameObject streak3;
    public GameObject Player;
    private PlayerCombat combat;
    private AudioSource audio;
    private Vector3 offset = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        combat = GetComponent<PlayerCombat>();
        offset.x += Player.GetComponent<BoxCollider2D>().bounds.size.x;
        audio = GetComponentsInChildren<AudioSource>()[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("altAttack") && Time.time >= nextCombo && Time.time >= combat.getNextAttackTime() && !combat.getCombo() && !PauseMenu.GameIsPaused)
		{
            //Debug.Log("Made it in");
            if(attackTracker == 0)
			{
                //Debug.Log("Attack 1");
                animator.SetTrigger("fast1");
                breakCombo = Time.time + attackBetween;
                attackTracker++;
                DealLightDamage();
                audio.Play();
                GameObject streakOne = Instantiate(streak1, Player.transform);
                Destroy(streakOne, 0.5f);
                
			}
            else if (attackTracker == 1)
			{
                //Debug.Log("Attack 2");
                animator.SetTrigger("fast2");
                breakCombo = Time.time + attackBetween;
                attackTracker++;
                DealLightDamage();
                audio.Play();
                GameObject streakTwo = Instantiate(streak2, Player.transform);
                Destroy(streakTwo, 0.5f);
            }
            else if (attackTracker == 2)
			{
                //Debug.Log("Attack 3");
                animator.SetTrigger("fast3");
                breakCombo = Time.time + attackBetween;
                attackTracker = 0;
                DealLightDamage();
                audio.Play();
                GameObject streakThree = Instantiate(streak3, Player.transform);
                Destroy(streakThree, 0.5f);
                nextCombo = Time.time + comboDelay;
            }
		}
        if(Time.time >= breakCombo)
		{
            attackTracker = 0;
            
		}
    }
    public void DealLightDamage()
    {
        List<GameObject> enemyObjects = new List<GameObject>();
        //Detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(combat.attackPoint.position, combat.attackRange, combat.enemyLayers);
        //Filter out each enemy, in the case that they have multiple colliders that were hit
        foreach (Collider2D collider in hitEnemies)
        {
            if (!enemyObjects.Contains(collider.gameObject))
            {
                enemyObjects.Add(collider.gameObject);
            }
        }
        //Apply damage to those enemies
        foreach (GameObject enemy in enemyObjects)
        {
            //Debug.Log("We hit " + enemy.name);
            if (enemy.tag == "Boss")
            {
                enemy.GetComponent<Boss>().TakeDamage((int)(staticVariables.lightDamage * staticVariables.damageMultiplier));
            }
            else
            {
                enemy.GetComponent<Enemy>().TakeDamage((int)(staticVariables.lightDamage * staticVariables.damageMultiplier));
            }
        }
    }
}

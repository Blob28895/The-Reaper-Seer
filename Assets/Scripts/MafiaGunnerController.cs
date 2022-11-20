using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiaGunnerController : MonoBehaviour
{
    // Used for movement 
    public float speed;
    public Transform target;
    public float minimumDistance;
    public float maximumDistance;
    private bool facingRight = true;
    private float dist;
    // Health and anim
    private Enemy enemy;
    private int currentHealth;
    public Animator animator;

    // Attack
    private GameObject player;
    private float timer;

    public GameObject bullet;
    public Transform firepoint;
    public float cooldown;    
    public float range;

    [SerializeField] private float colliderDistancex;
    [SerializeField] private float colliderDistancey;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    //Hit Stun
    public float hitStun = 0.6f;
    private float stunEnd = 0f;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        currentHealth = enemy.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        target = target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void setStunEnd()
    {
        animator.SetBool("Draw", false);
        stunEnd = Time.time + hitStun;
    }
    // Update is called once per frame
    void Update()
    {        
        float distance = Vector2.Distance(transform.position, player.transform.position);

        //float distance = Mathf.Abs(transform.position.x - player.transform.position.x);


        dist = target.position.x - transform.position.x;
        // Debug.Log(dist);
        if (dist > 0 && !facingRight)
        {
            Flip();
        }
        else if (dist < 0 && facingRight)
        {
            Flip();
        }

        if (enemy.getCurrentHealth() < currentHealth)
        {
            setStunEnd();
        }

        currentHealth = enemy.getCurrentHealth();
        // While the enemy is more than the minimum distance away from the player, the enemy will move toward the player
        if (Vector2.Distance(transform.position, target.position) > (range-1) && Vector2.Distance(transform.position, target.position) < maximumDistance)
        {
            animator.SetBool("Draw", false);
            animator.SetBool("Moving", true);
            Move();
        }
        else if (distance < range)
        {
            animator.SetBool("Draw", true);

            timer += Time.deltaTime;
            // To make sure enemy can face player if they dodge past them
            dist = target.position.x - transform.position.x; 
            if (dist > 0 && !facingRight)
            {
                Flip();
            }

            else if (dist < 0 && facingRight)
            {
                Flip();
            }
            // Attacks tied to animation events
            if(PlayerInSightFw())
            {
                if (timer > cooldown)
                {
                    timer = 0;
                    animator.SetTrigger("Attack");
                }
            }
            else if (PlayerInSightDw())
            {
                if (timer > cooldown)
                {
                    timer = 0;
                    animator.SetTrigger("AttackDown");
                }
            }
            else if (PlayerInSightUp())
            {
                if (timer > cooldown)
                {
                    timer = 0;
                    animator.SetTrigger("AttackUp");
                }
            }


        }
        else
        {
            animator.SetBool("Draw", false);
            animator.SetBool("Moving", false);
        }
    }

    //colliderDistancex -> box will start x distance away from char
    //colliderDistancey -> box will start y distance above from char
    bool PlayerInSightFw()  //First Overlap Box in front
    {
        Collider2D collider = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * (range+1) * transform.localScale.x * colliderDistancex,new Vector3(boxCollider.bounds.size.x * (range + 1), boxCollider.bounds.size.y, boxCollider.bounds.size.z),0,playerLayer);

        return collider != null;
    }
    bool PlayerInSightUp() //Second Overlap Box above char
    {
        Collider2D collider1 = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * (range + 1) * transform.localScale.x * colliderDistancex + transform.up * (range) * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * (range + 1), boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, playerLayer);

        return collider1 != null;
    }

    bool PlayerInSightDw() //Third Overlap Box below char
    {
        Collider2D collider2 = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * (range + 1) * transform.localScale.x * colliderDistancex - transform.up * (range) * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * (range + 1), boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, playerLayer);

        return collider2 != null;
    }


    private void OnDrawGizmos() //Gizmos for first, second and third Overlap Box respectively
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * (range + 1) * transform.localScale.x * colliderDistancex, new Vector3(boxCollider.bounds.size.x * (range + 1), boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * (range + 1) * transform.localScale.x * colliderDistancex + transform.up * (range) * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * (range + 1), boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * (range + 1) * transform.localScale.x * colliderDistancex - transform.up * (range) * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * (range + 1), boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


    void Shoot() //Shoot() will be called as an animation event for smoother anim
    {
        Instantiate(bullet, firepoint.position, Quaternion.identity);
    }

    private void Move()
    {
        //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Allows the enemy to do "hops" to be able to climb stairs when the Reaper is above it
        if (target.position.y > transform.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        // Allows the enmy to only move on the x axis to prevent him from going through the ground
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.position.x, transform.position.y), speed * Time.deltaTime);
        }
    }

    private void Flip() //Updated flip
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }
}

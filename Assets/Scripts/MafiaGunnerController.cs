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


    public float hitStun = 0.6f;
    private float stunEnd = 0f;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        currentHealth = enemy.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
 
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



        if (enemy.getCurrentHealth() < currentHealth)
        {
            setStunEnd();
        }

        currentHealth = enemy.getCurrentHealth();
        // While the enemy is more than the minimum distance away from the player, the enemy will move toward the player
        if (Vector2.Distance(transform.position, target.position) > (range + 1) && Vector2.Distance(transform.position, target.position) < maximumDistance)
        {
            animator.SetBool("Draw", false);
            animator.SetBool("Moving", true);
            Move();
        }
        else if (distance < range)
        {
            animator.SetBool("Draw", true);
            timer += Time.deltaTime;
            dist = target.position.x - transform.position.x;
            if (dist > 0 && !facingRight)
            {
                Flip();
            }

            else if (dist < 0 && facingRight)
            {
                Flip();
            }

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

    bool PlayerInSightFw()
    {
        Collider2D collider = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistancex,new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),0,playerLayer);

        return collider != null;
    }
    bool PlayerInSightUp()
    {
        Collider2D collider1 = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistancex + transform.up * range * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, playerLayer);

        return collider1 != null;
    }

    bool PlayerInSightDw()
    {
        Collider2D collider2 = Physics2D.OverlapBox(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistancex - transform.up * range * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, playerLayer);

        return collider2 != null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistancex, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistancex + transform.up * range * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistancex - transform.up * range * transform.localScale.x * colliderDistancey, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


    void Shoot()
    {
        Instantiate(bullet, firepoint.position, Quaternion.identity);
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }
}

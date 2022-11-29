using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperSeerBossController : MonoBehaviour
{
    public float speed;
    // Following are used for melee attacking
    public int attackDamage = 1;
    public float attackRange = 1f;
    public Transform attackPoint;
    public float attackRate = 1f;
    public LayerMask playerLayer;
    private List<string> meleeAnimations = new List<string>();
    private float nextAttackTime = 0f;
    // Used for slam attack
    public int slamDamage = 2;
    public float slamRange = 1.5f;
    public float slamKnockbackForce = 15f;
    public float slamKnockbackDuration = 0.2f;
    public Transform slamPoint;
    public float slamMinInterval = 6f;
    public float slamMaxInterval = 11f;
    private float nextSlamTime;
    // Used for grab attack
    public int grabDamage = 2;
    public float grabRange = 1f;
    public float grabKnockbackForce = 25f;
    public float grabKnockbackDuration = 0.3f;
    public Transform grabPoint;
    public Transform clawPosition;
    public float grabMinInterval = 9f;
    public float grabMaxInterval = 18f;
    private float nextGrabTime;
    private Collider2D grabbedPlayer;
    // Reaper stuff
    public Transform target;
    private float dist;
    // Used for Reaper Seer
    public Transform centerPivot;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Animator animator;
    private int maxHealth;
    private int health;
    private Boss boss;
    private float slowMult;
    private float slowTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boss = GetComponent<Boss>();
        maxHealth = boss.maxHealth;
        // Adds names of melee animations so that we can pick randomly
        meleeAnimations.Add("Melee1");
        meleeAnimations.Add("Melee2");
        // Next attack variables go here
        nextSlamTime = Time.time + Random.Range(slamMinInterval, slamMaxInterval);
        nextGrabTime = Time.time + Random.Range(grabMinInterval, grabMaxInterval);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(clawPosition.localPosition);
        /*if (Input.GetKeyDown(KeyCode.K))
        {
            float xDirection = dist / Mathf.Abs(dist);
            target.GetComponent<Player>().Knockback(new Vector2(xDirection * knockBackForce, knockBackForce / 2), knockBackDuration);
        }*/
        /*if (Input.GetKeyDown(KeyCode.I))
        {
            target.GetComponent<Player>().Immobolize(1f);
        }*/
        slowMult = boss.getSlowMult();
        slowTime = boss.getSlowTime();
        if (Time.time >= slowTime && slowMult != 1f)
        {
            slowMult = 1f;
        }
        health = boss.getCurrentHealth();
        dist = target.position.x - centerPivot.position.x;
        // Offset added since Reaper Seer's x changes when he flips
        if (dist > 0 && !facingRight)
        {
            Flip();
        }
        else if (dist < 0 && facingRight)
        {
            Flip();
        }
        // Move towards the Reaper while he's not in range
        if (CanMove() && !PlayerInRange(0))
        {
            animator.SetBool("Moving", true);
            MoveTowards();
        }
        // Slam attack
        else if (Time.time >= nextSlamTime && CanMove() && PlayerInRange(1))
        {
            animator.SetBool("Moving", false);
            StopMoving();
            animator.SetBool("Slam", true);
            float slamAttackTime = Random.Range(slamMinInterval, slamMaxInterval);
            nextSlamTime = Time.time + slamAttackTime;
        }
        // Grab attack
        else if (Time.time >= nextGrabTime && CanMove() && PlayerInRange(2))
        {
            animator.SetBool("Moving", false);
            StopMoving();
            animator.SetBool("TryGrab", true);
            float grabAttackTime = Random.Range(grabMaxInterval, grabMaxInterval);
            nextGrabTime = Time.time + grabAttackTime;
        }
        // The Reaper Seer will start attacking once the Reaper is in range
        else if (Time.time >= nextAttackTime && CanMove() && PlayerInRange(0))
        {
            animator.SetBool("Moving", false);
            StopMoving();
            //Debug.Log("Attacking!");
            int index = Random.Range(0, 2);
            //Attack();
            animator.SetBool(meleeAnimations[index], true);
            nextAttackTime = Time.time + 1f / attackRate;    
        }
        else
        {
            animator.SetBool("Moving", false);
            StopMoving();
        }
    }

    // Reaper Seer should only be able to move while not performing any kind of attack
    private bool CanMove()
    {
        return !animator.GetBool("Melee1") && !animator.GetBool("Melee2") && !animator.GetBool("Slam") && !animator.GetBool("TryGrab") && !animator.GetBool("GrabSuccessful");
    }

    // Detects whether the Reaper is in range or not to allow the Reaper Seer to perform melee attacks
    private bool PlayerInRange(int attackType)
    {
        Collider2D hitPlayer;
        // Slam Attack Range
        if (attackType == 1)
        {
            hitPlayer = Physics2D.OverlapCircle(slamPoint.position, slamRange, playerLayer);
            return hitPlayer != null;
        }
        // Grab Attack Range
        else if (attackType == 2)
        {
            hitPlayer = Physics2D.OverlapCircle(grabPoint.position, grabRange, playerLayer);
            return hitPlayer != null;
        }
        // Default to melee attack type if it's 0 or a number other than 1 or 2
        hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        return hitPlayer != null;
    }

    // Move towards the Reaper
    private void MoveTowards()
    {
        float xDirection = dist / Mathf.Abs(dist);
        rb.velocity = new Vector2(xDirection * speed * slowMult, rb.velocity.y);
    }

    // Set velocity to 0 so that the Reaper Seer doesn't just slide when he stops moving towards the Reaper
    private void StopMoving()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    // Deal damage to the Reaper if the attack hits him
    private void Attack()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }

    // Deal damage and knockback if Reaper is within range of the slam attack
    private void Slam()
    {
        Collider2D hitPlayer = Physics2D.OverlapCircle(slamPoint.position, slamRange, playerLayer);
        if (hitPlayer != null)
        {
            hitPlayer.GetComponent<Player>().TakeDamage(slamDamage);
            float xDirection = dist / Mathf.Abs(dist);
            hitPlayer.GetComponent<Player>().Knockback(new Vector2(xDirection * slamKnockbackForce, slamKnockbackForce / 2), slamKnockbackDuration);
        }
    }

    // Checks if the Reaper is within the grab range
    private void ReaperGrabbed()
    {
        clawPosition.localPosition = grabPoint.localPosition;
        grabbedPlayer = Physics2D.OverlapCircle(grabPoint.position, grabRange, playerLayer);
        if (grabbedPlayer != null)
        {
            animator.SetBool("GrabSuccessful", true);
            Debug.Log("Reaper has been grabbed!");
            // Immobilize the Reaper here
            grabbedPlayer.GetComponent<Player>().Immobolize(0.56f);
        }
        else
        {
            animator.SetBool("GrabSuccessful", false);
            Debug.Log("Reaper has NOT been grabbed!");
        }
    }

    private void UpdateClawPosition(float y)
    {
        clawPosition.localPosition = new Vector3(clawPosition.localPosition.x, y);
        grabbedPlayer.transform.position = clawPosition.position;
    }

    // Damage the Reaper and apply knockback to the Reaper when the final boss punches him in the grab attack
    private void GrabAttack()
    {
        animator.SetBool("GrabSuccessful", false);
        grabbedPlayer.GetComponent<Player>().TakeDamage(grabDamage);
        float xDirection = dist / Mathf.Abs(dist);
        grabbedPlayer.GetComponent<Player>().Knockback(new Vector2(xDirection * grabKnockbackForce, grabKnockbackForce / 2), grabKnockbackDuration);
        grabbedPlayer = null;
    }

    // Called as an animation event at the end of an attack animation to ensure that the Reaper Seer can move and use other attacks
    private void ResetBoolVar()
    {
        // Resets all bool variables after performing an attack to allow the head guard to move
        animator.SetBool("Melee1", false);
        animator.SetBool("Melee2", false);
        animator.SetBool("Slam", false);
        animator.SetBool("TryGrab", false);
    }

    // Flip the Reaper Seer so that he faces the Reaper at all times
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        // Move Reaper Seer's x coordinate since he's not centered in the sprite
        transform.position += (transform.localScale.x * -1) * new Vector3(0.45f, 0f);
    }

    // Show attack points in editor
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
        if (slamPoint != null)
        {
            Gizmos.DrawWireSphere(slamPoint.position, slamRange);
        }
        if (grabPoint != null)
        {
            Gizmos.DrawWireSphere(grabPoint.position, grabRange);
        }
    }
}

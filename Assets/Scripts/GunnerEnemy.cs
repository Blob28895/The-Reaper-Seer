using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerEnemy : MonoBehaviour
{
    public float speed;
    public float speed2;
    public Transform target;
    public float minimumDistance;
    public float maximumDistance;

    public GameObject projectile;
    public float timeBetweenShots;
    private float nextShotTime;

    private void Update()
    {
        if(Time.time > nextShotTime && Vector2.Distance(transform.position, target.position) < maximumDistance)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            nextShotTime = Time.time + timeBetweenShots;
        }

        if (Vector2.Distance(transform.position, target.position) < minimumDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, target.position) > minimumDistance + 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed2 * Time.deltaTime);
        }
    }
}

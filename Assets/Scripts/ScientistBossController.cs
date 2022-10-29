using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistBossController : MonoBehaviour
{
    public float speed;
    public float minimumDistance;
    //public float maximumDistance;
    public Transform target;
    public Transform throwPoint;
    public GameObject flask;
    public LayerMask playerLayer;
    public float throwRate = 0.5f;
    private float nextThrow = 0f;
    private bool jumping = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextThrow)
        {
            ThrowFlask();
            nextThrow = Time.time + 1f / throwRate;
        }
        if (!jumping && Vector2.Distance(transform.position, target.position) > minimumDistance + 0.5f)
        {
            MoveTowards();
        }
        else if (!jumping && Vector2.Distance(transform.position, target.position) < minimumDistance)
        {
            MoveAway();
        }
    }

    private void MoveTowards()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    private void MoveAway()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
    }

    private void Jump()
    {
        jumping = true;

        jumping = false;
    }

    private void ThrowFlask()
    {
       Instantiate(flask, throwPoint.position, Quaternion.identity);
    }

    private void SummonPoisonGas()
    {

    }
}

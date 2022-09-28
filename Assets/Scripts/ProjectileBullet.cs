using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBullet : MonoBehaviour
{
    Vector3 targetPosition;
    public float speed;


    private void Start()
    {
        targetPosition = FindObjectOfType<CharacterController2D>().transform.position;
    }


    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }
}

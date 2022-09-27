using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController2D : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 25;

    //attackRate is attacks per second
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    // Update is called once per frame
    void Update()
    {
        
    }
}

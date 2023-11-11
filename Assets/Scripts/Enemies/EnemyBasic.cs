using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
    public static Enemy Create(Vector3 position)
    {
        Transform pfenemy = Resources.Load<Transform>("EnemyBasic");
        Transform enemyTransform = Instantiate(pfenemy, position, Quaternion.identity);

        return enemyTransform.GetComponent<EnemyBasic>();
    }

    [SerializeField] private float speed;

    protected override void Awake()
    {
        base.Awake();
        hasMovementStarted = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void HandleMovement()
    {
        startMovementTimer -= Time.deltaTime;
        if (startMovementTimer <= 0f)
        {
            if (hasMovementStarted) return;
            Vector3 moveDirection = (Player.Instance.transform.position - transform.position).normalized;
            rigidBody2D.velocity = moveDirection * speed;
            hasMovementStarted = true;
        }
    }
}

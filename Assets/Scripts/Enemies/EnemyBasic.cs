using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : Enemy
{
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

            // rotate so that the top vertex of the triangle is pointing towards the player
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

            rigidBody2D.velocity = moveDirection * speed;
            hasMovementStarted = true;
        }
    }
}

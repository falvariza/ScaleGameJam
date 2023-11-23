using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : Enemy
{
    [SerializeField] private float speed;
    [SerializeField] private float targetFindRadius = 2f;
    [SerializeField] private float targetLeaveRadius = 3f;

    [SerializeField] private bool debugMode = false;

    private Transform targetTransform;

    protected override void Awake()
    {
        base.Awake();
        hasMovementStarted = false;
    }

    protected override void Update()
    {
        base.Update();
        LookForTargets();
    }

    protected override void HandleMovement()
    {
        startMovementTimer -= Time.deltaTime;
        if (startMovementTimer > 0f) return;

        if (hasMovementStarted && targetTransform == null) return;

        if (targetTransform != null)
        {
            if (Vector3.Distance(transform.position, targetTransform.position) > targetLeaveRadius)
            {
                targetTransform = null;
                return;
            }
        }

        Transform target = targetTransform == null ? Player.Instance.transform : targetTransform;
        Vector3 moveDirection = (target.position - transform.position).normalized;
        rigidBody2D.velocity = moveDirection * speed;
        hasMovementStarted = true;
    }

    private void OnDrawGizmos()
    {
        if (!debugMode) return;
        Gizmos.DrawSphere(transform.position, targetFindRadius);
    }

    private void LookForTargets()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, targetFindRadius);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Player player = collider2D.GetComponent<Player>();

            if (player != null)
            {
                if (targetTransform == null)
                {
                    targetTransform = player.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, player.transform.position) <
                        Vector3.Distance(transform.position, targetTransform.position))
                    {
                        targetTransform = player.transform;
                    }
                }
            }
        }
    }
}

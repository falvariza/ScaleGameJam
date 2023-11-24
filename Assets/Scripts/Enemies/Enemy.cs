using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxXDestroy = 20f;
    [SerializeField] private float maxYDestroy = 20f;
    [SerializeField] protected float startMovementTimerMax = .5f;
    [SerializeField] protected bool spawnsInsideBounds = false;

    protected float startMovementTimer;
    protected bool hasMovementStarted;

    protected Rigidbody2D rigidBody2D;

    protected virtual void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        startMovementTimer = startMovementTimerMax;
    }

    protected virtual void Update()
    {
        HandleDestroyOfflimits();
        HandleMovement();
    }

    protected virtual void HandleMovement()
    {
    }

    private void HandleDestroyOfflimits()
    {
        if (Math.Abs(transform.position.x) > maxXDestroy || Math.Abs(transform.position.y) > maxYDestroy)
        {
            Destroy(gameObject);
        }
    }

    public bool GetSpawnsInsideBounds()
    {
        return spawnsInsideBounds;
    }

}

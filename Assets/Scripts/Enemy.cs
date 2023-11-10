using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position)
    {
        Transform pfenemy = Resources.Load<Transform>("Enemy");
        Transform enemyTransform = Instantiate(pfenemy, position, Quaternion.identity);

        return enemyTransform.GetComponent<Enemy>();
    }

    [SerializeField] private float speed;
    [SerializeField] private float startMovementTimerMax = .5f;
    [SerializeField] private float maxXDestroy = 20f;
    [SerializeField] private float maxYDestroy = 20f;

    private Rigidbody2D rigidBody2D;
    private float startMovementTimer;
    private bool isStarted;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        isStarted = false;
    }

    private void Start()
    {
        startMovementTimer = startMovementTimerMax;
    }

    private void Update()
    {
        HandleMovement();
        HandleDestroy();
    }

    private void HandleMovement()
    {
        startMovementTimer -= Time.deltaTime;
        if (startMovementTimer <= 0f)
        {
            if (isStarted) return;
            Vector3 moveDirection = (Player.Instance.transform.position - transform.position).normalized;
            rigidBody2D.velocity = moveDirection * speed;
            isStarted = true;
        }
    }

    private void HandleDestroy()
    {
        if (Math.Abs(transform.position.x) > maxXDestroy || Math.Abs(transform.position.y) > maxYDestroy)
        {
            Destroy(gameObject);
        }
    }

}

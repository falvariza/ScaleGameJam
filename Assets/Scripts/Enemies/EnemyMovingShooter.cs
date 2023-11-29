using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingShooter : EnemyShooter
{
    [SerializeField] private float speed;

    private LevelManager.SpawnPosition spawnPosition;

    protected override void Awake()
    {
        base.Awake();
        hasMovementStarted = false;
    }

    protected override void Start()
    {
        base.Awake();
        SetSpawnPosition();
    }

    private void SetSpawnPosition()
    {
        SpawnBordersCoordinates spawnBordersCoordinates = LevelManager.Instance.GetSpawnBordersCoordinates();

        float x = transform.position.x;
        float y = transform.position.y;

        if (x == spawnBordersCoordinates.leftBorderSpawnX)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spawnPosition = LevelManager.SpawnPosition.Left;
        }
        else if (x == spawnBordersCoordinates.rightBorderSpawnX)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
            spawnPosition = LevelManager.SpawnPosition.Right;
        }
        else if (y == spawnBordersCoordinates.topBorderSpawnY)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            spawnPosition = LevelManager.SpawnPosition.Top;
        }
        else if (y == spawnBordersCoordinates.bottomBorderSpawnY)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            spawnPosition = LevelManager.SpawnPosition.Bottom;
        }
    }

    protected override void HandleMovement()
    {
        startMovementTimer -= Time.deltaTime;
        if (startMovementTimer <= 0f)
        {
            if (hasMovementStarted) return;
            Vector3 moveDirection = (Player.Instance.transform.position - transform.position).normalized;

            switch (spawnPosition)
            {
                case LevelManager.SpawnPosition.Left:
                case LevelManager.SpawnPosition.Right:
                    moveDirection.y = 0;
                    break;
                case LevelManager.SpawnPosition.Top:
                case LevelManager.SpawnPosition.Bottom:
                    moveDirection.x = 0;
                    break;
            }

            rigidBody2D.velocity = moveDirection * speed;
            hasMovementStarted = true;
        }
    }
}

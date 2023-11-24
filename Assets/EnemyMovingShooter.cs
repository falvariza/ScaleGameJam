using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingShooter : Enemy
{
    [SerializeField] private float speed;
    [SerializeField] private float shootTimerMax = 1f;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float shootingRange = 3f;

    private bool isShooting = false;
    private LevelManager.SpawnPosition spawnPosition;

    protected override void Awake()
    {
        base.Awake();
        hasMovementStarted = false;
    }

    private void Start()
    {
        bulletTransform.gameObject.SetActive(false);
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
            transform.rotation = Quaternion.Euler(0, 0, 0);
            spawnPosition = LevelManager.SpawnPosition.Right;
        }
        else if (y == spawnBordersCoordinates.topBorderSpawnY)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            spawnPosition = LevelManager.SpawnPosition.Top;
        }
        else if (y == spawnBordersCoordinates.bottomBorderSpawnY)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
            spawnPosition = LevelManager.SpawnPosition.Bottom;
        }
    }

    protected override void Update()
    {
        base.Update();
        HandleShooting();
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

    private void HandleShooting()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, shootingRange);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Player player = collider2D.GetComponent<Player>();
            if (player != null && !isShooting)
            {
                StartCoroutine(ShootPlayer(player));
            }
        }
    }

    private IEnumerator ShootPlayer(Player player)
    {
        while (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
        {
            isShooting = true;
            Vector3 shootDirection = (Player.Instance.transform.position - transform.position).normalized;
            Transform bullet = Instantiate(bulletTransform, transform.position, Quaternion.identity);
            bullet.gameObject.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
            yield return new WaitForSeconds(shootTimerMax);
        }

        isShooting = false;
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    private enum SpawnPosition
    {
        Left,
        Right,
        Top,
        Bottom
    }

    [SerializeField] private float leftBorderSpawnX;
    [SerializeField] private float rightBorderSpawnX;
    [SerializeField] private float topBorderSpawnY;
    [SerializeField] private float bottomBorderSpawnY;

    [SerializeField] private float spawnTimerMax = 1f;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnTimerMax;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            spawnTimer += spawnTimerMax;

            SpawnPosition spawnPosition = (SpawnPosition)Random.Range(0, 4);
            float randomX, randomY;

            switch(spawnPosition)
            {
                case SpawnPosition.Left:
                    randomX = leftBorderSpawnX;
                    randomY = Random.Range(bottomBorderSpawnY, topBorderSpawnY);
                    EnemyGenerator.GenerateRandomEnemyAt(new Vector3(randomX, randomY, 0));
                    break;
                case SpawnPosition.Right:
                    randomX = rightBorderSpawnX;
                    randomY = Random.Range(bottomBorderSpawnY, topBorderSpawnY);
                    EnemyGenerator.GenerateRandomEnemyAt(new Vector3(randomX, randomY, 0));
                    break;
                case SpawnPosition.Top:
                    randomX = Random.Range(leftBorderSpawnX, rightBorderSpawnX);
                    randomY = topBorderSpawnY;
                    EnemyGenerator.GenerateRandomEnemyAt(new Vector3(randomX, randomY, 0));
                    break;
                case SpawnPosition.Bottom:
                    randomX = Random.Range(leftBorderSpawnX, rightBorderSpawnX);
                    randomY = bottomBorderSpawnY;
                    EnemyGenerator.GenerateRandomEnemyAt(new Vector3(randomX, randomY, 0));
                    break;
            }
        }
    }
}

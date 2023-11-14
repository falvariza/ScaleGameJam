using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance { get; private set; }

    [SerializeField] private Transform powerUpReduceSizePrefab;
    [SerializeField] private int powerUpReduceSizeSpawnMaxCount = 3;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Player.Instance.GetSizeSystem().OnSizeIncreased += HandleSizeIncreased;
    }

    private void HandleSizeIncreased(object sender, SizeSystem.OnSizeIncreasedArgs e)
    {
        Vector3 playerPosition = Player.Instance.GetPlayerPosition();
        Vector3 playerSize = Player.Instance.GetColliderSize();

        float minSpawnRadius = playerSize.x / 4 + 1f;
        float maxSpawnRadius = minSpawnRadius + 3f * (1 - e.playerSize.size / 5);

        for(int i = 0; i < Random.Range(1, powerUpReduceSizeSpawnMaxCount); i++)
        {
            Vector3 spawnPosition = GenerateSpawnPosition(playerPosition, maxSpawnRadius, minSpawnRadius);
            spawnPosition = FixPositionInRange(spawnPosition, playerPosition, minSpawnRadius, maxSpawnRadius);
            Instantiate(powerUpReduceSizePrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GenerateSpawnPosition(Vector3 playerPosition, float maxSpawnRadius, float minSpawnRadius)
    {
        // Calculate a random angle in radians
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        // Calculate a random distance within the specified range
        float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Convert polar coordinates to Cartesian coordinates
        float spawnX = playerPosition.x + randomDistance * Mathf.Cos(randomAngle);
        float spawnY = playerPosition.y + randomDistance * Mathf.Sin(randomAngle);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        return spawnPosition;
    }


    Vector3 FixPositionInRange(Vector3 position, Vector3 playerPosition, float minSpawnRadius, float maxSpawnRadius)
    {
        SpawnBordersCoordinates borders = LevelManager.Instance.GetSpawnBordersCoordinates();
        float offset = 5f;

        float spawnCorrection = Random.Range(minSpawnRadius, maxSpawnRadius);

        if (position.x < borders.leftBorderSpawnX + offset)
        {
            position.x = playerPosition.x + spawnCorrection;
        }
        else if (position.x > borders.rightBorderSpawnX - offset)
        {
            position.x = playerPosition.x - spawnCorrection;
        }

        if (position.y < borders.bottomBorderSpawnY + offset)
        {
            position.y = playerPosition.y + spawnCorrection;
        }
        else if (position.y > borders.topBorderSpawnY - offset)
        {
            position.y = playerPosition.y - spawnCorrection;
        }

        return position;
    }
}

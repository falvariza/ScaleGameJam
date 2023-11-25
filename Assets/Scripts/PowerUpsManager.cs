using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance { get; private set; }

    [SerializeField] private Transform powerUpReduceSizePrefab;
    [SerializeField] private int powerUpReduceSizeSpawnMaxCount = 3;

    [SerializeField] private float respawnTimerMax = 5f;

    private float respawnTimer;
    private WaveConfigurationSO.PowerUpWaveConfiguration[] powerUpsWaveConfigurations;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Player.Instance.GetSizeSystem().OnSizeIncreased += HandleSizeIncreased;
        GameManager.Instance.OnCompleteLevel += HandleCompleteLevel;
    }

    private void Update()
    {
        if (Player.Instance.HasIncreasedSize())
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0f)
            {
                SpawnPowerUps();
                respawnTimer = respawnTimerMax;
            }
        }
    }

    private void HandleCompleteLevel(object sender, System.EventArgs e)
    {
        DestroyAllPowerUps();
    }

    private void HandleSizeIncreased(object sender, SizeSystem.OnSizeIncreasedArgs e)
    {
        SpawnPowerUps(Random.Range(1, powerUpReduceSizeSpawnMaxCount));
    }

    private void SpawnPowerUps(int count = 1)
    {

        for(int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GenerateSpawnPosition();
            Instantiate(powerUpReduceSizePrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 playerPosition = Player.Instance.GetPlayerPosition();
        Vector3 playerSize = Player.Instance.GetColliderSize();

        float minSpawnRadius = playerSize.x / 4 + 1f;
        float maxSpawnRadius = minSpawnRadius + 3f * (1 - Player.Instance.GetSizeSystem().CurrentSize.size / 5);

        // Calculate a random angle in radians
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);
        // Calculate a random distance within the specified range
        float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Convert polar coordinates to Cartesian coordinates
        float spawnX = playerPosition.x + randomDistance * Mathf.Cos(randomAngle);
        float spawnY = playerPosition.y + randomDistance * Mathf.Sin(randomAngle);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0);

        return FixPositionInRange(spawnPosition, playerPosition, minSpawnRadius, maxSpawnRadius);
    }

    private Vector3 FixPositionInRange(Vector3 position, Vector3 playerPosition, float minSpawnRadius, float maxSpawnRadius)
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

    private void DestroyAllPowerUps()
    {
        PowerUp[] powerUps = FindObjectsOfType<PowerUp>();

        foreach (PowerUp powerUp in powerUps)
        {
            Destroy(powerUp.gameObject);
        }
    }

    private IEnumerator SpawnPowerUpWave(WaveConfigurationSO.PowerUpWaveConfiguration powerUpWaveConfiguration)
    {
        while (GameManager.Instance.IsGamePlaying())
        {
            for (int i = 0; i < powerUpWaveConfiguration.maxNumberOfPowerUpsPerSpawn; i++)
            {
                Transform powerUpPrefab = powerUpWaveConfiguration.powerUpsPrefabs[Random.Range(0, powerUpWaveConfiguration.powerUpsPrefabs.Length)];
                Vector3 spawnPosition = GenerateSpawnPosition();
                Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(powerUpWaveConfiguration.spawnInterval);
        }
        yield break;
    }

    public void ResetPowerUps()
    {
        DestroyAllPowerUps();
    }

    public void StartPowerUpWave(WaveConfigurationSO.PowerUpWaveConfiguration[] powerUpsWaveConfigurations)
    {
        this.powerUpsWaveConfigurations = powerUpsWaveConfigurations;

        foreach (WaveConfigurationSO.PowerUpWaveConfiguration powerUpWaveConfiguration in powerUpsWaveConfigurations)
        {
            StartCoroutine(SpawnPowerUpWave(powerUpWaveConfiguration));
        }
    }

    public void StopPowerUpWave()
    {
        StopAllCoroutines();
    }
}

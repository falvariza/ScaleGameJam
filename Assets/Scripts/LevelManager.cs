using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private enum SpawnPosition
    {
        Left,
        Right,
        Top,
        Bottom
    }

    [SerializeField] private SpawnBordersCoordinates spawnBordersCoordinates;

    private LevelConfigurationSO levelConfiguration;
    private int currentWaveIndex = 0;
    private float spawnTimer;

    private WaveConfigurationSO[] waveConfigurations;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnCompleteLevel += HandleCompleteLevel;
    }

    private void HandleCompleteLevel(object sender, System.EventArgs e)
    {
        DestroyAllEnemies();
        currentWaveIndex = 0;
        Player.Instance.ResetPlayerPosition();
    }

    private void Update()
    {
        if (levelConfiguration == null) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            spawnTimer += waveConfigurations[currentWaveIndex].spawnInterval;
            SpawnWave();
            spawnTimer = waveConfigurations[currentWaveIndex].spawnInterval + Random.Range(-waveConfigurations[currentWaveIndex].spawnIntervalRandomness, waveConfigurations[currentWaveIndex].spawnIntervalRandomness);
        }

        if (GetNextWave() != null && GetNextWave().waveStartingTime <= GameManager.Instance.GetTranscurringPlayingTime())
        {
            StartNextWave();
        }
    }

    private void SpawnWave()
    {
        if (GameManager.Instance.IsGameOver())
        {
            return;
        }

        var currentWave = waveConfigurations[currentWaveIndex];

        for (int i = 0; i < Random.Range(1, currentWave.maxNumberOfEnemiesPerSpawn); i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Transform enemyPrefab = currentWave.enemiesPrefabs[Random.Range(0, currentWave.enemiesPrefabs.Length)];
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void StartNextWave()
    {
        currentWaveIndex++;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        SpawnPosition spawnPosition = (SpawnPosition)Random.Range(0, 4);
        float randomX, randomY;

        switch (spawnPosition)
        {
            case SpawnPosition.Left:
                randomX = spawnBordersCoordinates.leftBorderSpawnX;
                randomY = Random.Range(spawnBordersCoordinates.bottomBorderSpawnY, spawnBordersCoordinates.topBorderSpawnY);
                break;
            case SpawnPosition.Right:
                randomX = spawnBordersCoordinates.rightBorderSpawnX;
                randomY = Random.Range(spawnBordersCoordinates.bottomBorderSpawnY, spawnBordersCoordinates.topBorderSpawnY);
                break;
            case SpawnPosition.Top:
                randomX = Random.Range(spawnBordersCoordinates.leftBorderSpawnX, spawnBordersCoordinates.rightBorderSpawnX);
                randomY = spawnBordersCoordinates.topBorderSpawnY;
                break;
            case SpawnPosition.Bottom:
                randomX = Random.Range(spawnBordersCoordinates.leftBorderSpawnX, spawnBordersCoordinates.rightBorderSpawnX);
                randomY = spawnBordersCoordinates.bottomBorderSpawnY;
                break;
            default:
                return Vector3.zero;
        }

        return new Vector3(randomX, randomY, 0);
    }

    private WaveConfigurationSO GetCurrentWave()
    {
        return waveConfigurations.Length > currentWaveIndex ? waveConfigurations[currentWaveIndex] : null;
    }

    private WaveConfigurationSO GetNextWave()
    {
        return waveConfigurations.Length > currentWaveIndex + 1 ? waveConfigurations[currentWaveIndex + 1] : null;
    }

    public SpawnBordersCoordinates GetSpawnBordersCoordinates()
    {
        return spawnBordersCoordinates;
    }

    public void StartLevel(LevelConfigurationSO levelConfiguration)
    {
        this.levelConfiguration = levelConfiguration;
        waveConfigurations = levelConfiguration.wavesConfigurations;
        spawnTimer = waveConfigurations[currentWaveIndex].spawnInterval;
    }

    private void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }
}

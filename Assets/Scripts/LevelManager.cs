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
        this.currentWaveIndex = 0;
        this.levelConfiguration = null;

        Player.Instance.ResetPlayerPosition();
    }

    private void Update()
    {
        if (levelConfiguration == null) return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            WaveConfigurationSO currentWave = GetCurrentWave();
            spawnTimer += currentWave.spawnInterval;
            SpawnWave();
            spawnTimer = currentWave.spawnInterval + Random.Range(-currentWave.spawnIntervalRandomness, currentWave.spawnIntervalRandomness);
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

        WaveConfigurationSO currentWave = GetCurrentWave();

        for (int i = 0; i < Random.Range(1, currentWave.maxNumberOfEnemiesPerSpawn); i++)
        {
            Transform enemyPrefab = currentWave.enemiesPrefabs[Random.Range(0, currentWave.enemiesPrefabs.Length)];
            bool spawnsInsideBounds = enemyPrefab.GetComponent<Enemy>().GetSpawnsInsideBounds();
            Vector3 spawnPosition = GetRandomSpawnPosition(spawnsInsideBounds);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void StartNextWave()
    {
        currentWaveIndex++;
    }

    private Vector3 GetRandomSpawnPosition(bool spawnsInsideBounds)
    {
        if (spawnsInsideBounds)
        {
            return GetInsideBoundsSpawnPosition();
        }
        else
        {
            return GetOutsideBoundsSpawnPosition();
        }
    }

    private Vector3 GetInsideBoundsSpawnPosition()
    {
        // get spawn position inside bounds with an offset of 2f, and that is farther than 3f from the player
        Vector3 spawnPosition = new Vector3(
            Random.Range(spawnBordersCoordinates.leftBorderSpawnX + 2f, spawnBordersCoordinates.rightBorderSpawnX - 2f),
            Random.Range(spawnBordersCoordinates.bottomBorderSpawnY + 2f, spawnBordersCoordinates.topBorderSpawnY - 2f),
            0
        );

        if (Vector3.Distance(spawnPosition, Player.Instance.transform.position) < 3f)
        {
            // return a position that is greater that moves the spawn position away from the player, towards the center of the screen
            return spawnPosition + (spawnPosition - Player.Instance.transform.position).normalized * 3f;
        }

        return spawnPosition;
    }

    private Vector3 GetOutsideBoundsSpawnPosition()
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

    private void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    public SpawnBordersCoordinates GetSpawnBordersCoordinates()
    {
        return spawnBordersCoordinates;
    }

    public void StartLevel(LevelConfigurationSO levelConfiguration)
    {
        this.levelConfiguration = levelConfiguration;
        this.currentWaveIndex = 0;
        waveConfigurations = levelConfiguration.wavesConfigurations;
        spawnTimer = GetCurrentWave().spawnInterval;
    }


    public void ResetLevel()
    {
        DestroyAllEnemies();
        this.currentWaveIndex = 0;
        this.levelConfiguration = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public enum SpawnPosition
    {
        Left,
        Right,
        Top,
        Bottom
    }

    [SerializeField] private SpawnBordersCoordinates spawnBordersCoordinates;

    private LevelConfigurationSO levelConfiguration;
    private int currentWaveIndex = 0;
    private bool isFirstWaveStarted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnCompleteLevel += HandleCompleteLevel;
        GameManager.Instance.OnCompleteFullLevel += HandleCompleteFullLevel;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void HandleCompleteLevel(object sender, System.EventArgs e)
    {
        DestroyAllEnemies();
        this.currentWaveIndex = 0;
        this.levelConfiguration = null;

        Player.Instance.ResetPlayerPosition();
        CameraHandler.Instance.ResetCameraSize(() => {
            GameManager.Instance.StartNextLevel();
        });
    }

    private void HandleCompleteFullLevel(object sender, System.EventArgs e)
    {
        DestroyAllEnemies();
        this.currentWaveIndex = 0;
        this.levelConfiguration = null;

        Player.Instance.ResetPlayerPosition();
        CameraHandler.Instance.ResetCameraSize();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        GameManager.State gameState = GameManager.Instance.GetState();
        if (gameState == GameManager.State.GameOver || gameState == GameManager.State.CompletedLevel)
        {
            StopWaves();
        }

        if (gameState == GameManager.State.GameOver)
        {
            DestroyAllEnemies();
            CameraHandler.Instance.CameraShake(2f);
        }
    }

    private void Update()
    {
        if (levelConfiguration == null) return;

        if (!isFirstWaveStarted)
        {
            isFirstWaveStarted = true;
            StartCurrentWave();
        }

        if (GetNextWave() != null && GetNextWave().waveStartingTime <= GameManager.Instance.GetTranscurringPlayingTime())
        {
            StartNextWave();
        }
    }

    private IEnumerator StartEnemyWave(WaveConfigurationSO.EnemyWaveConfiguration enemyWaveConfiguration)
    {
        while (GameManager.Instance.IsGamePlaying())
        {
            for (int i = 0; i < Random.Range(1, enemyWaveConfiguration.maxNumberOfEnemiesPerSpawn); i++)
            {
                Transform enemyPrefab = enemyWaveConfiguration.enemiesPrefabs[Random.Range(0, enemyWaveConfiguration.enemiesPrefabs.Length)];
                bool spawnsInsideBounds = enemyPrefab.GetComponent<Enemy>().GetSpawnsInsideBounds();
                Vector3 spawnPosition = GetRandomSpawnPosition(spawnsInsideBounds);
                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            }
            float spawnTimer = enemyWaveConfiguration.spawnInterval + Random.Range(-enemyWaveConfiguration.spawnIntervalRandomness, enemyWaveConfiguration.spawnIntervalRandomness);
            yield return new WaitForSeconds(spawnTimer);
        }

        yield break;
    }

    private void StartCurrentWave()
    {
        WaveConfigurationSO.EnemyWaveConfiguration[] enemyWaveConfigurations = GetCurrentWave().enemiesWaveConfigurations;

        foreach (WaveConfigurationSO.EnemyWaveConfiguration enemyWaveConfiguration in enemyWaveConfigurations)
        {
            StartCoroutine(StartEnemyWave(enemyWaveConfiguration));
            PowerUpsManager.Instance.StartPowerUpWave(GetCurrentWave().powerUpsWaveConfigurations);
        }
    }

    private void StartNextWave()
    {
        currentWaveIndex++;
        StopWaves();

        StartCurrentWave();
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
        float cornerOffset = 1f;

        switch (spawnPosition)
        {
            case SpawnPosition.Left:
                randomX = spawnBordersCoordinates.leftBorderSpawnX;
                randomY = Random.Range(spawnBordersCoordinates.bottomBorderSpawnY + cornerOffset, spawnBordersCoordinates.topBorderSpawnY - cornerOffset);
                break;
            case SpawnPosition.Right:
                randomX = spawnBordersCoordinates.rightBorderSpawnX;
                randomY = Random.Range(spawnBordersCoordinates.bottomBorderSpawnY + cornerOffset, spawnBordersCoordinates.topBorderSpawnY - cornerOffset);
                break;
            case SpawnPosition.Top:
                randomX = Random.Range(spawnBordersCoordinates.leftBorderSpawnX + cornerOffset, spawnBordersCoordinates.rightBorderSpawnX - cornerOffset);
                randomY = spawnBordersCoordinates.topBorderSpawnY;
                break;
            case SpawnPosition.Bottom:
                randomX = Random.Range(spawnBordersCoordinates.leftBorderSpawnX + cornerOffset, spawnBordersCoordinates.rightBorderSpawnX - cornerOffset);
                randomY = spawnBordersCoordinates.bottomBorderSpawnY;
                break;
            default:
                return Vector3.zero;
        }

        return new Vector3(randomX, randomY, 0);
    }

    private WaveConfigurationSO GetCurrentWave()
    {
        return GetWaveConfiguration().Length > currentWaveIndex ? GetWaveConfiguration()[currentWaveIndex] : null;
    }

    private WaveConfigurationSO GetNextWave()
    {
        return GetWaveConfiguration().Length > currentWaveIndex + 1 ? GetWaveConfiguration()[currentWaveIndex + 1] : null;
    }

    private void DestroyAllEnemies()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    private WaveConfigurationSO[] GetWaveConfiguration()
    {
        if (levelConfiguration == null) return null;
        return levelConfiguration.wavesConfigurations;
    }

    private void StopWaves()
    {
        StopAllCoroutines();
        PowerUpsManager.Instance.StopPowerUpWave();
    }

    public SpawnBordersCoordinates GetSpawnBordersCoordinates()
    {
        return spawnBordersCoordinates;
    }

    public void StartLevel(LevelConfigurationSO levelConfiguration)
    {
        this.levelConfiguration = levelConfiguration;
        this.currentWaveIndex = 0;
        this.isFirstWaveStarted = false;
    }


    public void ResetLevel()
    {
        DestroyAllEnemies();
        this.currentWaveIndex = 0;
        this.levelConfiguration = null;
        this.isFirstWaveStarted = false;
    }
}

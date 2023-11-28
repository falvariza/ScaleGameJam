using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public static PowerUpsManager Instance { get; private set; }

    [SerializeField] private Transform powerUpReduceSizePrefab;
    [SerializeField] private int powerUpReduceSizeSpawnMaxCount = 3;

    [SerializeField] private float respawnPowerUpReduceSizeTimerMax = 5f;

    private float respawnPowerUpReduceSizeTimer;
    private WaveConfigurationSO.PowerUpWaveConfiguration[] powerUpsWaveConfigurations;
    private Dictionary<PowerUpSO, float> activePowerUpsDurationDictionary = new Dictionary<PowerUpSO, float>();

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
            respawnPowerUpReduceSizeTimer -= Time.deltaTime;

            if (respawnPowerUpReduceSizeTimer <= 0f)
            {
                int powerUpCount = 1 + (int)(Player.Instance.GetSizeSystem().CurrentSize.size / 5) * 2;
                SpawnReduceSizePowerUps(powerUpCount);
                respawnPowerUpReduceSizeTimer = respawnPowerUpReduceSizeTimerMax + 1 * (1 - Player.Instance.GetSizeSystem().CurrentSize.size / 5);
            }
        }

        // HandleActivePowerUps();
    }

    private void HandleActivePowerUps()
    {
        List<PowerUpSO> powerUpsToRemove = new List<PowerUpSO>();

        foreach (KeyValuePair<PowerUpSO, float> activePowerUp in activePowerUpsDurationDictionary)
        {
            float duration = activePowerUp.Value - Time.deltaTime;

            activePowerUpsDurationDictionary[activePowerUp.Key] = duration;

            if (activePowerUp.Value <= 0f)
            {
                powerUpsToRemove.Add(activePowerUp.Key);
            }
        }

        foreach (PowerUpSO powerUpToRemove in powerUpsToRemove)
        {
            activePowerUpsDurationDictionary.Remove(powerUpToRemove);
        }
    }

    private void HandleCompleteLevel(object sender, System.EventArgs e)
    {
        DestroyAllPowerUps();
    }

    private void HandleSizeIncreased(object sender, SizeSystem.OnSizeIncreasedArgs e)
    {
        SpawnReduceSizePowerUps(Random.Range(1, powerUpReduceSizeSpawnMaxCount));
    }

    private void SpawnReduceSizePowerUps(int count = 1)
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
        float maxSpawnRadius = minSpawnRadius + 4f * (1 - Player.Instance.GetSizeSystem().CurrentSize.size / 5);

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
                PowerUpSO powerUpSO = powerUpWaveConfiguration.powerUps[Random.Range(0, powerUpWaveConfiguration.powerUps.Length)];
                Transform powerUpPrefab = powerUpSO.powerUpPrefab;

                Vector3 spawnPosition = GenerateSpawnPosition();
                Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

                // float powerUpDuration = powerUpPrefab.GetComponent<PowerUp>().GetPowerUpDuration();
                // if (powerUpDuration > 0f)
                // {
                //     AddActivePowerUp(powerUpSO, powerUpDuration);
                // }
            }
            yield return new WaitForSeconds(powerUpWaveConfiguration.spawnInterval);
        }
        yield break;
    }

    private void AddActivePowerUp(PowerUpSO powerUpSO, float duration)
    {
        if (activePowerUpsDurationDictionary.ContainsKey(powerUpSO))
        {
            activePowerUpsDurationDictionary[powerUpSO] = duration;
        }
        else
        {
            activePowerUpsDurationDictionary.Add(powerUpSO, duration);
        }
    }

    public void ResetPowerUps()
    {
        DestroyAllPowerUps();
    }

    public void StartPowerUpWave(WaveConfigurationSO.PowerUpWaveConfiguration[] powerUpsWaveConfigs)
    {
        this.powerUpsWaveConfigurations = powerUpsWaveConfigs;

        foreach (WaveConfigurationSO.PowerUpWaveConfiguration powerUpWaveConfiguration in powerUpsWaveConfigurations)
        {
            StartCoroutine(SpawnPowerUpWave(powerUpWaveConfiguration));
        }
    }

    public void StopPowerUpWave()
    {
        StopAllCoroutines();
    }

    public Dictionary<PowerUpSO, float> GetActivePowerUps()
    {
        return activePowerUpsDurationDictionary;
    }

}

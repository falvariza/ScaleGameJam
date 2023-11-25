using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfiguration", menuName = "ScriptableObjects/WaveConfigurationSO")]
public class WaveConfigurationSO : ScriptableObject
{
    [System.Serializable]
    public struct EnemyWaveConfiguration
    {
        public Transform[] enemiesPrefabs;
        public int maxNumberOfEnemiesPerSpawn;
        public float spawnInterval;
        public float spawnIntervalRandomness;
    }

    [System.Serializable]
    public struct PowerUpWaveConfiguration
    {
        public Transform[] powerUpsPrefabs;
        public int maxNumberOfPowerUpsPerSpawn;
        public float spawnInterval;
        public float spawnIntervalRandomness;
    }

    public EnemyWaveConfiguration[] enemiesWaveConfigurations;
    public PowerUpWaveConfiguration[] powerUpsWaveConfigurations;
    public float waveStartingTime;
}

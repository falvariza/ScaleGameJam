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

    public EnemyWaveConfiguration[] enemiesWaveConfigurations;
    public float waveStartingTime;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfiguration", menuName = "ScriptableObjects/WaveConfigurationSO")]
public class WaveConfigurationSO : ScriptableObject
{
    public Transform[] enemiesPrefabs;
    public float spawnInterval;
    public float spawnIntervalRandomness;
    public int maxNumberOfEnemiesPerSpawn;

}

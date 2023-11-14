using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfiguration", menuName = "ScriptableObjects/LevelConfigurationSO")]
public class LevelConfigurationSO : ScriptableObject
{
    public WaveConfigurationSO[] wavesConfigurations;
    public float levelDuration;
}

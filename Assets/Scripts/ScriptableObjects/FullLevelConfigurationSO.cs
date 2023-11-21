using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FullLevelConfigurationSO", menuName = "ScriptableObjects/FullLevelConfigurationSO")]
public class FullLevelConfigurationSO : ScriptableObject
{
    public LevelConfigurationSO[] levelsConfigurations;
    public SceneLoader.Scene levelScene;
    public Color levelColor;
}

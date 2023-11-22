using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsSelectorManager : MonoBehaviour
{
    public static LevelsSelectorManager Instance { get; private set; }

    [SerializeField] private FullLevelConfigurationSO[] levels;
    [SerializeField] private int currentLevelIndex = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;
        SceneLoader.Load(levels[levelIndex].levelScene);
    }

    public FullLevelConfigurationSO[] GetAllLevels()
    {
        return levels;
    }

    public FullLevelConfigurationSO GetCurrentFullLevel()
    {
        return levels[currentLevelIndex];
    }
}

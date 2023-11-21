using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsSelectorManager : MonoBehaviour
{
    public static LevelsSelectorManager Instance { get; private set; }

    [SerializeField] private FullLevelConfigurationSO[] levels;

    private int currentLevelIndex = 0;

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
        GameManager.Instance.StartLevel(levels[currentLevelIndex]);
    }

    public FullLevelConfigurationSO[] GetAllLevels()
    {
        return levels;
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < levels.Length)
        {
            SceneLoader.Load(levels[currentLevelIndex].levelScene);
        }
    }
}

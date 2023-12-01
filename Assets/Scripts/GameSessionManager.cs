using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance { get; private set; }

    public static int MaxFullLevelCompleted { get; private set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            MaxFullLevelCompleted = PlayerPrefs.GetInt("MaxFullLevelCompleted", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentLevel(int level)
    {
        MaxFullLevelCompleted = level;
    }

    public void IncreaseLevelInHistory()
    {
        MaxFullLevelCompleted++;
        PlayerPrefs.SetInt("MaxFullLevelCompleted", MaxFullLevelCompleted);
    }

    public void ResetLevel()
    {
        MaxFullLevelCompleted = 0;
    }
}

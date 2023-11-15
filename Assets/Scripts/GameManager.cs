using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStarted;
    public event EventHandler OnGameOver;
    public event EventHandler OnCompleteLevel;
    public event EventHandler OnCompleteFullLevel;

    [SerializeField] private FullLevelConfigurationSO fullLevelConfiguration;

    private int currentLevelIndex = 0;
    private float gamePlayingTimerMax;
    private bool isGameStarted = false;

    private float gamePlayingTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartGame(); // Temp
    }

    private void Update()
    {
        HandleGamePlayingTimer();
    }

    private void HandleGamePlayingTimer()
    {
        if (!isGameStarted) return;

        gamePlayingTimer -= Time.deltaTime;

        if (gamePlayingTimer <= 0f)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        isGameStarted = false;
        Time.timeScale = 0f;

        if (currentLevelIndex == fullLevelConfiguration.levelsConfigurations.Length - 1)
        {
            OnCompleteFullLevel?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnCompleteLevel?.Invoke(this, EventArgs.Empty);
            StartNextLevel(); // Temp
        }
    }

    private LevelConfigurationSO GetCurrentLevelConfiguration()
    {
        return fullLevelConfiguration.levelsConfigurations[currentLevelIndex];
    }

    public void StartGame()
    {
        LevelConfigurationSO currentLevelConfiguration = GetCurrentLevelConfiguration();
        Time.timeScale = 1f;
        gamePlayingTimerMax = currentLevelConfiguration.levelDuration;
        gamePlayingTimer = gamePlayingTimerMax;
        isGameStarted = true;
        LevelManager.Instance.StartLevel(currentLevelConfiguration);
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGameOver()
    {
        return gamePlayingTimer <= 0f;
    }

    public void StartNextLevel()
    {
        currentLevelIndex++;
        StartGame();
    }

    public void GameOver()
    {
        isGameStarted = false;
        Time.timeScale = 0f;
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    public float GetGamePlayingCountdownInSeconds()
    {
        return Mathf.Ceil(gamePlayingTimer);
    }

    public float GetTranscurringPlayingTime()
    {
        return Mathf.Round(gamePlayingTimerMax - gamePlayingTimer);
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
}

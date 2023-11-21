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
    public event EventHandler OnStateChanged;
    public event EventHandler OnRestartGame;

    [SerializeField] private FullLevelConfigurationSO fullLevelConfiguration;

    private enum State
    {
        Idle,
        CountdownToStart,
        GamePlaying,
        CompletedLevel,
        GameOver,
    }

    private State gameState;
    private int currentLevelIndex = 0;
    private float gamePlayingTimerMax;
    private float countdownToStartTimerMax = 3f;
    private float countdownToStartTimer;

    private float gamePlayingTimer;

    private void Awake()
    {
        Instance = this;
        gameState = State.Idle;
    }

    private void Start()
    {
        StartGame(); // Temp
    }

    private void Update()
    {
        HandleGameState();
    }

    private void HandleGameState()
    {
        switch (gameState)
        {
            case State.Idle:
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;

                if (countdownToStartTimer < 0f)
                {
                    LevelConfigurationSO currentLevelConfiguration = GetCurrentLevelConfiguration();
                    gamePlayingTimerMax = currentLevelConfiguration.levelDuration;
                    gamePlayingTimer = gamePlayingTimerMax;
                    gamePlayingTimer = gamePlayingTimerMax;
                    LevelManager.Instance.StartLevel(currentLevelConfiguration);
                    UpdateState(State.GamePlaying);
                }
                break;
            case State.GamePlaying:
                HandleGamePlayingTimer();
                break;
            case State.GameOver:
                break;
            default:
                break;
        }
    }

    private void HandleGamePlayingTimer()
    {
        gamePlayingTimer -= Time.deltaTime;

        if (gamePlayingTimer <= 0f)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        Time.timeScale = 0f;

        UpdateState(State.CompletedLevel);

        if (currentLevelIndex == fullLevelConfiguration.levelsConfigurations.Length - 1)
        {
            PlayerProgress.IncreaseLevel();
            OnCompleteFullLevel?.Invoke(this, EventArgs.Empty);
        }
    }

    private LevelConfigurationSO GetCurrentLevelConfiguration()
    {
        return fullLevelConfiguration.levelsConfigurations[currentLevelIndex];
    }

    private void UpdateState(State newState)
    {
        gameState = newState;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void StartGameCountdown()
    {
        Time.timeScale = 1f;
        countdownToStartTimer = countdownToStartTimerMax;
        UpdateState(State.CountdownToStart);
    }

    public void StartLevel(FullLevelConfigurationSO fullLevelConfigurationSO)
    {
        fullLevelConfiguration = fullLevelConfigurationSO;
        currentLevelIndex = 0;
        StartGame();
    }

    public void StartGame()
    {
        StartGameCountdown();
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGameOver()
    {
        return gameState == State.GameOver;
    }

    public void StartNextLevel()
    {
        OnCompleteLevel?.Invoke(this, EventArgs.Empty);
        currentLevelIndex++;
        StartGame();
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UpdateState(State.GameOver);
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

    public bool IsGamePlaying()
    {
        return gameState == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return gameState == State.CountdownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public bool IsLevelCompleted()
    {
        return gameState == State.CompletedLevel;
    }

    public bool IsFullLevelCompleted()
    {
        return IsLevelCompleted() && currentLevelIndex == fullLevelConfiguration.levelsConfigurations.Length - 1;
    }

    public string GetLevelProgressText()
    {
        return $"{currentLevelIndex + 1}/{fullLevelConfiguration.levelsConfigurations.Length}";
    }

    public void RestartGame()
    {
        currentLevelIndex = 0;

        OnRestartGame?.Invoke(this, EventArgs.Empty);
        Player.Instance.ResetPlayer();
        LevelManager.Instance.ResetLevel();
        PowerUpsManager.Instance.ResetPowerUps();
        StartGame();
    }
}

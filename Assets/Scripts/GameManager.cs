using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStarted;
    public event EventHandler OnGameOver;

    private float gamePlayingTimerMax;
    private bool isGameStarted = false;

    private float gamePlayingTimer;

    private void Awake()
    {
        Instance = this;
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
            GameOver();
        }
    }

    public void StartGame(float gamePlayingTimerMax)
    {
        Time.timeScale = 1f;
        this.gamePlayingTimerMax = gamePlayingTimerMax;
        gamePlayingTimer = gamePlayingTimerMax;
        isGameStarted = true;
        OnGameStarted?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGameOver()
    {
        return gamePlayingTimer <= 0f;
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

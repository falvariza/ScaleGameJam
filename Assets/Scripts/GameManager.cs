using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float gamePlayingTimerMax = 15f;

    private float gamePlayingTimer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gamePlayingTimer = gamePlayingTimerMax;
    }

    private void Update()
    {
        gamePlayingTimer -= Time.deltaTime;

        Debug.Log(GetGamePlayingTimerNormalized());

        if (gamePlayingTimer <= 0f)
        {
            // Game Over
            Debug.Log("Game Over");
            Time.timeScale = 0f;
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }

    public float GetGamePlayingTimeInSeconds()
    {
        return Mathf.Round(gamePlayingTimer);
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

}

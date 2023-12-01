using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelCompletedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI levelProgressText;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button exitButton;

    private float gameOverUIDelay = 3f;

    private void Awake()
    {
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void Update()
    {
        levelProgressText.text = GameManager.Instance.GetLevelProgressText();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLevelCompleted())
        {
            ShowLevelCompleted();
        } else if (GameManager.Instance.IsGameOver())
        {
            Invoke(nameof(ShowGameOver), gameOverUIDelay);
        }
        else {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowLevelCompleted()
    {
        bool isFullLevelCompleted = GameManager.Instance.IsFullLevelCompleted();
        titleText.text = isFullLevelCompleted ? "Level Completed" : "Finished wave";
        if (isFullLevelCompleted)
        {
            levelProgressText.gameObject.SetActive(false);
            nextLevelButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
            nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Replay";
            exitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Continue";
        }
        else
        {
            levelProgressText.gameObject.SetActive(true);
            nextLevelButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    private void ShowGameOver()
    {
        titleText.text = "Game Over";
        levelProgressText.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
        gameObject.SetActive(true);
    }

    private void OnNextLevelButtonClicked()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsFullLevelCompleted())
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            GameManager.Instance.StartNextLevel();
        }
    }

    private void OnExitButtonClicked()
    {
        GameManager.Instance.NavigateToLevelSelectorMenu();
    }
}

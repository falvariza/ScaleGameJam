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

    private void Awake()
    {
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
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
            ShowGameOver();
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
        Debug.Log("Lets show level completed");
        titleText.text = "Level Completed";
        levelProgressText.gameObject.SetActive(true);
        string nextLevelText = GameManager.Instance.IsFullLevelCompleted() ? "Replay" : "Next Level";
        nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = nextLevelText;
        gameObject.SetActive(true);
    }

    private void ShowGameOver()
    {
        Debug.Log("Lets show game over");
        titleText.text = "Game Over";
        levelProgressText.gameObject.SetActive(false);
        nextLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = "Retry";
        gameObject.SetActive(true);
    }

    private void OnNextLevelButtonClicked()
    {
        Debug.Log("Next level button clicked");
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsFullLevelCompleted())
        {
            Debug.Log("Restart game sent");
            GameManager.Instance.RestartGame();
        }
        else
        {
            Debug.Log("Start next level");
            GameManager.Instance.StartNextLevel();
        }
    }
}

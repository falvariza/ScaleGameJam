using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Transform resumeButton;
    [SerializeField] private Transform exitButton;

    private void Start()
    {
        Hide();
        resumeButton.GetComponent<Button>().onClick.AddListener(OnResumeButtonClicked);
        exitButton.GetComponent<Button>().onClick.AddListener(OnExitButtonClicked);
        GameManager.Instance.OnPauseGame += GameManager_OnPauseGame;
        GameManager.Instance.OnResumeGame += GameManager_OnResumeGame;
    }

    private void GameManager_OnResumeGame(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnPauseGame(object sender, System.EventArgs e)
    {
        Show();
    }

    private void OnResumeButtonClicked()
    {
        GameManager.Instance.ResumeGame();
        Hide();
    }

    private void OnExitButtonClicked()
    {
        GameManager.Instance.NavigateToLevelSelectorMenu();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}

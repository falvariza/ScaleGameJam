using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedUI : MonoBehaviour
{
    [SerializeField] private Button nextLevelButton;

    private void Awake()
    {
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsLevelCompleted())
        {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void OnNextLevelButtonClicked()
    {
        GameManager.Instance.StartNextLevel();
    }
}

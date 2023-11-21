using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }

    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        Instance = this;
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void Start()
    {
        if (MainMenuStaticData.ShowLevelSelectorUI)
        {
            MainMenuStaticData.ShowLevelSelectorUI = false;
            LevelSelectorUI.Instance.Show();
            Hide();
        }
        else {
            Show();
        }
    }

    private void OnStartButtonClicked()
    {
        LevelSelectorUI.Instance.Show();
        Hide();
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}

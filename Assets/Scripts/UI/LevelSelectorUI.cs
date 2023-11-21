using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    public static LevelSelectorUI Instance { get; private set; }

    [SerializeField] private Button levelButtonTemplate;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Transform buttonsContainer;


    private void Awake()
    {
        Instance = this;
        Hide();
    }

    private void Start()
    {
        backToMenuButton.onClick.AddListener(OnBackToMenuButtonClicked);
        GenerateLevelButtons();
    }

    private void OnBackToMenuButtonClicked()
    {
        MainMenuUI.Instance.Show();
        Hide();
    }

    private void GenerateLevelButtons()
    {
        FullLevelConfigurationSO[] levels = LevelsSelectorManager.Instance.GetAllLevels();

        levelButtonTemplate.gameObject.SetActive(false);
        for (int i = 0; i < levels.Length; i++)
        {
            Button levelButton = Instantiate(levelButtonTemplate, buttonsContainer);
            levelButton.gameObject.SetActive(true);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Level {i + 1}";
            int levelIndex = i;
            levelButton.onClick.AddListener(() => OnLevelButtonClicked(levelIndex));
            if (GameSessionManager.MaxFullLevelCompleted >= i)
            {
                levelButton.interactable = true;
            }
            else
            {
                levelButton.interactable = false;
            }
        }
    }

    private void OnLevelButtonClicked(int levelIndex)
    {
        SceneLoader.Load(LevelsSelectorManager.Instance.GetAllLevels()[levelIndex].levelScene);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

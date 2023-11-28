using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Transform livesContainer;
    [SerializeField] private Image lifeImagePrefab;
    [SerializeField] private Transform powerUpContainer;
    [SerializeField] private Transform powerUpItemTemplate;

    private void Start()
    {
        powerUpItemTemplate.gameObject.SetActive(false);
    }

    private void Update()
    {
        countdownText.text = GameManager.Instance.GetGamePlayingCountdownInSeconds().ToString();
        ShowLives();
        // ShowPowerUps();1
    }

    private void ShowLives()
    {
        int lives = Player.Instance.GetPlayerLife();
        int livesContainerCount = livesContainer.childCount;

        if (lives > livesContainerCount)
        {
            for (int i = livesContainerCount; i < lives; i++)
            {
                Image lifeImage = Instantiate(lifeImagePrefab, livesContainer);
                lifeImage.color = Color.white;
            }
        }
        else if (lives < livesContainerCount)
        {
            for (int i = livesContainerCount - 1; i >= lives; i--)
            {
                Destroy(livesContainer.GetChild(i).gameObject);
            }
        }
    }

    private void ShowPowerUps()
    {
        Dictionary<PowerUpSO, float> activePowerUps = PowerUpsManager.Instance.GetActivePowerUps();

        // destroy all children of powerUpContainer
        foreach (Transform child in powerUpContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<PowerUpSO, float> activePowerUp in activePowerUps)
        {
            Transform powerUpItemTransform = Instantiate(powerUpItemTemplate, powerUpContainer);
            powerUpItemTransform.gameObject.SetActive(true);

            Image powerUpImage = powerUpItemTransform.Find("Image").GetComponent<Image>();
            powerUpImage.sprite = activePowerUp.Key.powerUpIcon;

            TextMeshProUGUI powerUpDurationText = powerUpItemTransform.Find("Duration").GetComponent<TextMeshProUGUI>();
            powerUpDurationText.text = activePowerUp.Value.ToString("F1");
        }
    }
}

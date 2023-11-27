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

    private void Update()
    {
        countdownText.text = GameManager.Instance.GetGamePlayingCountdownInSeconds().ToString();
        ShowLives();
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
}

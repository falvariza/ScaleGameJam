using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Update()
    {
        countdownText.text = GameManager.Instance.GetGamePlayingCountdownInSeconds().ToString();
    }
}

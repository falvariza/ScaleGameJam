using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Play();
        GameManager.Instance.OnRestartGame += GameManager_OnRestartGame;
    }

    private void GameManager_OnRestartGame(object sender, System.EventArgs e)
    {
        Play();
    }

    private void Play()
    {
        FullLevelConfigurationSO fullLevelConfiguration = LevelsSelectorManager.Instance.GetCurrentFullLevel();
        GetComponent<AudioSource>().clip = fullLevelConfiguration.audioClip;
        GetComponent<AudioSource>().Play();
    }

    public void Stop()
    {
        GetComponent<AudioSource>().Stop();
    }

}

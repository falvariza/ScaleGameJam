using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private void Start()
    {
        FullLevelConfigurationSO fullLevelConfiguration = LevelsSelectorManager.Instance.GetCurrentFullLevel();
        GetComponent<AudioSource>().clip = fullLevelConfiguration.audioClip;
        GetComponent<AudioSource>().Play();
    }

}

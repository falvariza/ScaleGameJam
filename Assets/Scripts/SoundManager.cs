using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefs;

    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volume);
    }

    public void PlayExplosionSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefs.explosion, position, volume);
    }

    public void PlayHitSound(Vector3 position, float volume = 3f)
    {
        PlaySound(audioClipRefs.hit, position, volume);
    }
}

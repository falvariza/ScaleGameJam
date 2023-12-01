using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private float orthographicSize;
    private Vector3 originalPosition;
    private CinemachineBasicMultiChannelPerlin cbmcp;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        orthographicSize = virtualCamera.m_Lens.OrthographicSize;
        originalPosition = virtualCamera.transform.position;
    }

    public void LerpDownCameraSize(float playingTime)
    {
        float newOrthographicSize = Mathf.Lerp(orthographicSize / 2f, orthographicSize, playingTime);

        virtualCamera.m_Lens.OrthographicSize = newOrthographicSize;

        // start following player
        virtualCamera.Follow = Player.Instance.transform;
    }

    public void ResetCameraSize(Action callback = null)
    {
        StartCoroutine(LerpUpCameraSize(2f, callback));
    }

    private IEnumerator LerpUpCameraSize(float duration, Action callback = null)
    {
        float timer = 0f;
        float startOrthographicSize = virtualCamera.m_Lens.OrthographicSize;

        while (timer < duration || virtualCamera.transform.position != originalPosition)
        {
            timer += Time.deltaTime;
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startOrthographicSize, orthographicSize, timer / duration);
            yield return null;
        }

        virtualCamera.Follow = null;

        callback?.Invoke();
    }

    public void CameraShake(float duration = 1f)
    {
        cbmcp = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cbmcp.m_AmplitudeGain = 1f;

        StartCoroutine(StopCameraShake(duration));
    }

    private IEnumerator StopCameraShake(float duration)
    {
        yield return new WaitForSeconds(duration);
        cbmcp.m_AmplitudeGain = 0f;
    }
}

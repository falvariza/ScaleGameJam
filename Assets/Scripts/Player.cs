using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public EventHandler OnExpandingSizeStarted;
    public EventHandler OnExpandingSizeFinished;

    private SizeSystem sizeSystem;

    private float playerScale;
    private float targetScale;
    private float scalingDuration = .5f;
    private float playerInitialScale;

    private void Awake()
    {
        sizeSystem = GetComponent<SizeSystem>();
        Instance = this;
        playerScale = sizeSystem.CurrentSize.size;
        playerInitialScale = transform.localScale.x;
        sizeSystem.OnSizeIncreased += SizeSystem_OnSizeIncreased;
        sizeSystem.OnSizeDecreased += SizeSystem_OnSizeDecreased;
    }

    private void SizeSystem_OnSizeDecreased(object sender, System.EventArgs e)
    {
        targetScale = sizeSystem.CurrentSize.size;
        StartCoroutine(ScalePlayer());
    }

    private void SizeSystem_OnSizeIncreased(object sender, SizeSystem.OnSizeIncreasedArgs e)
    {
        OnExpandingSizeStarted?.Invoke(this, EventArgs.Empty);
        targetScale = e.playerSize.size;
        StartCoroutine(ScalePlayer());
    }

    private IEnumerator ScalePlayer()
    {
        float timer = 0f;
        float startScale = playerScale;
        bool isExpanding = targetScale > startScale;

        while (timer < scalingDuration)
        {
            timer += Time.deltaTime;
            playerScale = Mathf.Lerp(startScale, targetScale, timer / scalingDuration);
            if (!GameManager.Instance.IsGamePlaying())
            {
                yield break;
            }
            yield return null;
        }

        playerScale = targetScale;

        if (isExpanding)
        {
            OnExpandingSizeFinished?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        float newScale = playerInitialScale * playerScale;
        transform.localScale = new Vector3(newScale, newScale, 1);
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (sizeSystem.IsExploded() || !GameManager.Instance.IsGamePlaying()) return;

        float speed = sizeSystem.GetSpeed();
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            Destroy(enemy.gameObject);
            sizeSystem.IncreaseSize();

            if (sizeSystem.IsExploded()) {
                GameManager.Instance.GameOver();
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }

    public SizeSystem GetSizeSystem()
    {
        return GetComponent<SizeSystem>();
    }

    public Vector3 GetColliderSize()
    {
        return GetComponent<CircleCollider2D>().bounds.size;
    }

    public void ResetPlayerPosition()
    {
        transform.position = Vector3.zero;
    }

    public void ResetPlayer()
    {
        // TODO: this can be refactored to send an event that the other components listen
        sizeSystem.ResetSize();
        ResetPlayerPosition();
        playerScale = sizeSystem.CurrentSize.size;
        targetScale = playerScale;
        GetBlastAttackSystem().Reset();
        StopCoroutine(ScalePlayer());
    }

    public bool HasIncreasedSize()
    {
        return sizeSystem.IsExploded() || sizeSystem.CurrentSize.size > 1;
    }

    public float GetActualPlayerScale()
    {
        return playerInitialScale * playerScale;
    }

    public float GetTargetScale()
    {
        return targetScale;
    }

    public BlastAttackSystem GetBlastAttackSystem()
    {
        return GetComponent<BlastAttackSystem>();
    }

    public int GetPlayerLife()
    {
        return sizeSystem.GetLife();
    }

}

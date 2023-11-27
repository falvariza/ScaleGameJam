using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
public class BlastAttackSystem : MonoBehaviour
{
    public EventHandler OnBlastRadiusChanged;
    public EventHandler OnBlastEnemiesEnded;

    private float blastRadius;
    private SizeSystem sizeSystem;
    private Player player;

    private void Start()
    {
        sizeSystem = GetComponent<SizeSystem>();
        player = GetComponent<Player>();
        sizeSystem.OnSizeIncreased += SizeSystem_OnSizeIncreased;
        sizeSystem.OnSizeIncreased += SizeSystem_OnSizeIncreased;
    }

    private void SizeSystem_OnSizeIncreased(object sender, SizeSystem.OnSizeIncreasedArgs e)
    {
        StartCoroutine(BlastNearbyEnemies());
    }

    private IEnumerator BlastNearbyEnemies()
    {
        float timer = 0f;
        float blastDuration = .5f;
        float playerRadius = player.GetActualPlayerScale() / 2;

        while (timer < blastDuration)
        {
            timer += Time.deltaTime;
            float blastRadiusMax = playerRadius + .5f + player.GetTargetScale() * .2f;
            blastRadius = Mathf.Lerp(playerRadius, blastRadiusMax, timer / blastDuration);
            OnBlastRadiusChanged?.Invoke(this, EventArgs.Empty);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, blastRadius);
            foreach (Collider2D collider in colliders)
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            if (!GameManager.Instance.IsGamePlaying())
            {
                OnBlastEnemiesEnded?.Invoke(this, EventArgs.Empty);
                yield break;
            }
            yield return null;
        }

        blastRadius = player.GetActualPlayerScale() / 2;
        OnBlastEnemiesEnded?.Invoke(this, EventArgs.Empty);
    }

    public float GetBlastRadius()
    {
        return blastRadius;
    }

    public void Reset()
    {
        StopAllCoroutines();
        blastRadius = player.GetActualPlayerScale() / 2;
    }
}

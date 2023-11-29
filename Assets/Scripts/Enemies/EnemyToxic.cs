using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyToxic : Enemy
{
    [SerializeField] private float reachRadius = 2f;
    [SerializeField] private float lifespanMax = 5f;
    [SerializeField] private float damageTimerMax = 2f;
    [SerializeField] private Transform toxicCloudTransform;

    private float lifespan;
    private bool hasDamagedPlayer;

    protected override void Awake()
    {
        base.Awake();
        lifespan = lifespanMax;
    }

    protected override void Update()
    {
        toxicCloudTransform.localScale = new Vector3(reachRadius * 2, reachRadius * 2, 1f);

        base.Update();
        HandleLifespan();
        HandleDamageCycle();
        SearchForPlayerInReach();
    }

    protected override void HandleMovement()
    {
        // Do nothing
    }

    private void HandleLifespan()
    {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void HandleDamageCycle()
    {
        if (!hasDamagedPlayer) return;
        damageTimerMax -= Time.deltaTime;
        if (damageTimerMax <= 0f)
        {
            hasDamagedPlayer = false;
            damageTimerMax = 2f;
        }
    }

    private void SearchForPlayerInReach()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, reachRadius);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Player player = collider2D.GetComponent<Player>();

            if (player != null && !hasDamagedPlayer)
            {
                player.GetSizeSystem().IncreaseSize();
                hasDamagedPlayer = true;
            }
        }
    }

    private void OnDrawGizmos() {
        // Gizmos.DrawSphere(transform.position, reachRadius);
    }

}

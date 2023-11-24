using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFixedShooter : EnemyShooter
{
    [SerializeField] private float lifespanMax = 5f;

    private float lifespan;

    protected override void Awake()
    {
        base.Awake();
        lifespan = lifespanMax;
    }

    protected override void Update()
    {
        base.Update();
        HandleLifespan();
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
}

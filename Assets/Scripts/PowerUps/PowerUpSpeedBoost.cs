using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeedBoost : PowerUp
{
    [SerializeField] private float speedBoostMultiplier = .5f;

    protected override void PowerUpAction()
    {
        base.PowerUpAction();

        SizeSystem playerSizeSystem = player.GetSizeSystem();
        playerSizeSystem.AddSpeedBoost(speedBoostMultiplier);
    }

    protected override void PowerUpHasExpired()
    {
        base.PowerUpHasExpired();
        if (player != null)
        {
            player.GetSizeSystem().RemoveSpeedBoost();
            player = null;
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.GetSizeSystem().RemoveSpeedBoost();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpReduceSize : PowerUp
{
    protected override void PowerUpAction()
    {
        base.PowerUpAction();
        playerSizeSystem.DecreaseSize();
    }
}

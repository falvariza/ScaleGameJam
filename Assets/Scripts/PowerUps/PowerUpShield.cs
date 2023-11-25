using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : PowerUp
{
    [SerializeField] protected Transform shieldPrefab;

    private Transform shield;
    protected override void PowerUpAction()
    {
        base.PowerUpAction();
        StartCoroutine(ShieldPowerUp());
    }

    IEnumerator ShieldPowerUp()
    {
        shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);
        shield.SetParent(player.transform);
        shield.localPosition = Vector3.zero;

        shield.localScale = Vector3.one * 2;
        shield.localRotation = Quaternion.identity;

        yield return new WaitForSeconds(powerUpDuration);

        Destroy(gameObject);
    }

    private void OnDestroy() {
        if(shield != null)
        {
            Destroy(shield.gameObject);
        }
    }
}

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

        foreach (Shield existingShield in player.GetComponentsInChildren<Shield>())
        {
            existingShield.gameObject.SetActive(false);
            Destroy(existingShield.gameObject);
        }

        shield = Instantiate(shieldPrefab, player.transform.position, Quaternion.identity);
        shield.SetParent(player.transform);
        shield.localPosition = Vector3.zero;

        shield.localScale = Vector3.one * 2;
        shield.localRotation = Quaternion.identity;
    }

    protected override void PowerUpHasExpired()
    {
        base.PowerUpHasExpired();

        if(shield != null)
        {
            shield.gameObject.SetActive(false);
            Destroy(shield.gameObject);
        }
    }

    private void OnDestroy()
    {
        if(shield != null)
        {
            shield.gameObject.SetActive(false);
            Destroy(shield.gameObject);
        }
    }
}

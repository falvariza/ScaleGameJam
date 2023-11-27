using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSword : PowerUp
{
    [SerializeField] protected Transform swordPrefab;

    private Transform sword;

    protected override void PowerUpAction()
    {
        base.PowerUpAction();

        foreach (Sword existingSword in player.GetComponentsInChildren<Sword>())
        {
            existingSword.gameObject.SetActive(false);
            Destroy(existingSword.gameObject);
        }

        sword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        sword.SetParent(player.transform);
        sword.localPosition = Vector3.zero;
        sword.localScale = new Vector3(.15f, 2f, 1f);
    }

    protected override void PowerUpHasExpired()
    {
        base.PowerUpHasExpired();

        if(sword != null)
        {
            sword.gameObject.SetActive(false);
            Destroy(sword.gameObject);
        }
    }

    private void OnDestroy()
    {
        if(sword != null)
        {
            sword.gameObject.SetActive(false);
            Destroy(sword.gameObject);
        }
    }
}

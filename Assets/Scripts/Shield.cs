using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void Update()
    {
        float playerScale = Player.Instance.GetActualPlayerScale();
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, playerScale);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if(enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}

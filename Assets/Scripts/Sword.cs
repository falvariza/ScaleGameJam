using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float speed = .5f;
    [SerializeField] private float swingCycle = 1f;

    private float swingTimer = 0f;

    private void Update()
    {
        swingTimer += Time.deltaTime;
        if(swingTimer >= swingCycle)
        {
            swingTimer = 0f;
        }

        float swingProgress = swingTimer / speed;
        float swingAngle = Mathf.Lerp(0f, 360f, swingProgress);
        transform.localRotation = Quaternion.Euler(0f, 0f, swingAngle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if(enemy != null)
        {
            Destroy(enemy.gameObject);
        }
    }
}

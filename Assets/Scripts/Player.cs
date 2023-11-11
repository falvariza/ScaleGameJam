using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    private SizeSystem sizeSystem;

    private void Awake()
    {
        sizeSystem = GetComponent<SizeSystem>();
        Instance = this;
    }

    private void Update()
    {
        if (sizeSystem.IsExploded()) return;

        float size = sizeSystem.CurrentSize.size;
        float speed = sizeSystem.CurrentSize.speed;
        transform.localScale = new Vector3(size, size, 1);

        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            Destroy(enemy.gameObject);
            sizeSystem.IncreaseSize();

            if (sizeSystem.IsExploded()) {
                GameManager.Instance.GameOver();
            }
        }
    }
}

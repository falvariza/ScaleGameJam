using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [SerializeField] protected float shootTimerMax = 1f;
    [SerializeField] protected Transform bulletTransform;
    [SerializeField] protected float bulletSpeed = 5f;
    [SerializeField] protected float shootingRange = 3f;

    protected bool isShooting = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        bulletTransform.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        HandleShooting();
    }

    protected void HandleShooting()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, shootingRange);

        foreach (Collider2D collider2D in collider2Ds)
        {
            Player player = collider2D.GetComponent<Player>();
            if (player != null && !isShooting)
            {
                StartCoroutine(ShootPlayer(player));
            }
        }
    }

    protected IEnumerator ShootPlayer(Player player)
    {
        while (Vector3.Distance(transform.position, player.transform.position) <= shootingRange)
        {
            isShooting = true;
            Vector3 shootDirection = (Player.Instance.transform.position - transform.position).normalized;
            Transform bullet = Instantiate(bulletTransform, transform.position, Quaternion.identity);
            bullet.gameObject.SetActive(true);
            bullet.GetComponent<Rigidbody2D>().velocity = shootDirection * bulletSpeed;
            yield return new WaitForSeconds(shootTimerMax);
        }

        isShooting = false;
        yield return null;
    }
}

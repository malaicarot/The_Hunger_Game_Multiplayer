using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Bullet_Pistol,
    Bullet_Rifle,
    Bullet_Sniper,
    Bullet_Shotgun,
}

[RequireComponent(typeof(PooledObject))]
public class Shooting : PooledObject
{
    BulletType type;
    [SerializeField] Transform firePoint;
    Controller player;

    void Start()
    {
        player = FindObjectOfType<Controller>();

    }
    void Update()
    {
        if (gameObject.name.ToString() != "Bomb")
        {
            float fire = Input.GetAxis("Fire1");
            if (Input.GetButtonDown("Fire1") && fire != 0)
            {
                Shoot();
            }
        }
        else
        {
            StartCoroutine(Explosion());
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);

        Explode explode = ExplosionPool.SingletonExplosionPool.Explode("Explosion", transform.position, transform.rotation);
        explode.EndExplode();
        Release();
    }

    void Shoot()
    {
        switch (gameObject.name)
        {
            case "Pistol":
                type = BulletType.Bullet_Pistol;
                break;
            case "Rifle":
                type = BulletType.Bullet_Rifle;
                break;
            case "Sniper":
                type = BulletType.Bullet_Sniper;
                break;
            case "Shotgun":
                type = BulletType.Bullet_Shotgun;
                break;
        }

        BulletShooter bullet = BulletPool.SingletonBulletPool.GetBullet(type.ToString(), firePoint.position, Quaternion.identity);
        if (bullet != null)
        {
            Vector2 direction = -firePoint.right;
            bullet.InitializeBullet(direction);
        }
    }
}

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

void Start(){
    firePoint.rotation = Quaternion.Euler(0, 0, 0);
}
    void Update()
    {
        float fire = Input.GetAxis("Fire1");
        if (Input.GetButtonDown("Fire1") && fire != 0)
        {
            Shoot();
        }

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
        }

        BulletShooter bullet = BulletPool.SingletonBulletPool.GetBullet(type.ToString(), firePoint.position, Quaternion.Euler(0, 0, 0));
        if (bullet != null)
        {
            Vector2 direction = firePoint.right;
            if(transform.localScale.x > 0){
                direction = -firePoint.right;
            }
            
            bullet.InitializeBullet(direction);

        }
        

    }
}

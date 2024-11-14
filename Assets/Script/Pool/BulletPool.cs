using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : ObjectPool
{
    public static BulletPool SingletonBulletPool;
    void Awake()
    {
        SingletonBulletPool = this;
        
    }

    public BulletShooter GetBullet(string ItemType, Vector3 position, Quaternion quaternion){
        GameObject objOfPool = SingletonBulletPool.GetPooledObject(ItemType);
        BulletShooter Bullet = objOfPool.GetComponent<BulletShooter>();
        Bullet.transform.position = position;
        Bullet.transform.rotation = quaternion;
        return Bullet;
    }

    public override void ReturnToPool(GameObject pooledObject)
    {
        base.ReturnToPool(pooledObject);
    }


}

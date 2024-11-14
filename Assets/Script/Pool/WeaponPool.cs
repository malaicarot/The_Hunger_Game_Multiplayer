using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponPool : ObjectPool
{
    public static WeaponPool SingletonWeaponPool;
    void Awake()
    {
        SingletonWeaponPool = this;
    }

    public Shooting CreateWeapon(WeaponType weaponType, Vector3 position, Quaternion quaternion)
    {
        GameObject objOfPool = SingletonWeaponPool.GetPooledObject(weaponType.ToString());
        Shooting weapon = objOfPool.GetComponent<Shooting>();
        weapon.transform.position = position;
        weapon.transform.rotation = quaternion;
        return weapon;
    }

    public override void ReturnToPool(GameObject pooledObject)
    {
        base.ReturnToPool(pooledObject);
    }
}

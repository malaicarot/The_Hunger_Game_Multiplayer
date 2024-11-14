using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : ObjectPool
{
    public static ExplosionPool SingletonExplosionPool;

    void Awake(){
        SingletonExplosionPool = this;
    }

    public Explode Explode(string explode, Vector3 position, Quaternion quaternion){
        GameObject objOfPool = SingletonExplosionPool.GetPooledObject(explode);
        Explode explosion = objOfPool.GetComponent<Explode>();
        explosion.transform.position = position;
        explosion.transform.rotation = quaternion;
        return explosion;

    }
    public override void ReturnToPool(GameObject pooledObject)
    {
        base.ReturnToPool(pooledObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class Explode : PooledObject
{

    public void EndExplode(){
        StartCoroutine(ReturnExplosion());
    }
    IEnumerator ReturnExplosion(){
        yield return new WaitForSeconds(0.8f);
        Release();
    }
}

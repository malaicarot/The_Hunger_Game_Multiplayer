using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    private ObjectPool pool;
    public ObjectPool _pool {get => pool; set => pool = value;}

    public void Release(){
        if(_pool != null){
            _pool.ReturnToPool(this);
        }
    }
  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemPool : ObjectPool
{
    public static ItemPool SingletonItemPool;
    void Awake()
    {
        SingletonItemPool = this;
        
    }

    public ItemMovement GetItem(ItemType ItemType, Vector3 position, Quaternion quaternion){
        PooledObject objOfPool = SingletonItemPool.GetPooledObject(ItemType.ToString());
        ItemMovement Item = objOfPool.GetComponent<ItemMovement>();
        Item.transform.position = position;
        Item.transform.rotation = quaternion;
        return Item;
    }

}

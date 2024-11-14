using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ObjectPool : MonoBehaviourPun
{
    [Range(10, 100)][SerializeField] private int poolSize;
    [SerializeField] private List<GameObject> objectToPool;
    private Dictionary<string, Stack<GameObject>> poolDictionary;
    void Start()
    {
        SetupPool();
    }

    void SetupPool()
    {
        //Neu khong co Object nao hoac List khong co doi tuong nao thi kh lam gi het
        if (objectToPool == null || objectToPool.Count == 0)
        {
            return;
        }
        poolDictionary = new Dictionary<string, Stack<GameObject>>();
        //Duyet qua List Object
        foreach (var obj in objectToPool)
        {
            Stack<GameObject> objStack = new Stack<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                //Sinh ra so luong obj dua tren poolSize
                GameObject instance = PhotonNetwork.Instantiate(obj.name, transform.position, transform.rotation, 0);
                // instance._pool = this;
                instance.gameObject.name = obj.name;
                instance.gameObject.SetActive(false);
                objStack.Push(instance);
            }
            poolDictionary.Add(obj.name, objStack);
        }
    }

    public GameObject GetPooledObject(string objType)
    {
        if (string.IsNullOrEmpty(objType) || !poolDictionary.ContainsKey(objType))
        {
            Debug.Log($"DON'T FIND OBJ TYPE {objType}");
        }

        //Neu trong ho da het thi tao ra obj moi

        if (poolDictionary[objType].Count == 0)
        {
            GameObject newInstance = PhotonNetwork.Instantiate(objectToPool.Find(obj => obj.name == objType)?.name, transform.position, transform.rotation, 0);            
            newInstance.gameObject.name = objType;
            // newInstance._pool = this;
            return newInstance;
        }

        // Lay Obj o dau Stack
        GameObject nextInstance = poolDictionary[objType].Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;

    }

    public virtual void ReturnToPool(GameObject pooledObject)
    {
        if (pooledObject == null)
        {
            Debug.LogError("PooledObject is null and cannot be returned to the pool.");
            return;

        }
        if (!poolDictionary.ContainsKey(pooledObject.name))
        {
            Debug.Log(pooledObject.name);
            PhotonNetwork.Destroy(pooledObject.gameObject);

        }
        else
        {
            poolDictionary[pooledObject.name].Push(pooledObject);
            pooledObject.gameObject.SetActive(false);
        }
    }
}

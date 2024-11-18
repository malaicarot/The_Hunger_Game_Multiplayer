using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public class ItemSpawn : MonoBehaviourPun
{
    [SerializeField] float appearTime = 7f;
    [SerializeField] GameObject[] ItemsPrefabs;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ItemSpawnTime());
        }
    }

    IEnumerator ItemSpawnTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(appearTime);
            ItemSpawner();
        }
    }

    void ItemSpawner()
    {
        float randomX = Random.Range(-20f, 20f);
        int randomItem = Random.Range(0, 5);
        Vector3 itemSpawner = new Vector3(randomX, 0.3f, 0f);
        GameObject item = PhotonNetwork.Instantiate(ItemsPrefabs[randomItem].name, itemSpawner, Quaternion.identity);
        item.name = ItemsPrefabs[randomItem].name;
    }
}

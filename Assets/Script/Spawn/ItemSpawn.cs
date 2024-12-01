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
        float randomX = Random.Range(-29f, 29f);
        float randomY = Random.Range(10f, -12f);
        int randomItem = Random.Range(0, 5);
        Vector3 itemSpawner = new Vector3(randomX, randomY, 0f);
        GameObject item = PhotonNetwork.Instantiate(ItemsPrefabs[randomItem].name, itemSpawner, Quaternion.identity);
        item.name = ItemsPrefabs[randomItem].name;
    }
}

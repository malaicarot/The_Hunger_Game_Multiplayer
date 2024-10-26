using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Item_1,
    Item_2,
    Item_3,
    Item_4,
    Item_5,

}
public class ItemSpawn : MonoBehaviour
{
    [SerializeField] float appearTime = 7f;
    [SerializeField] ItemType itemType;
    void Start()
    {
        StartCoroutine(ItemSpawnTime());
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
        float randomX = Random.Range(-8f, 8f);
        int randomItem = Random.Range(1, 4);
        Vector3 itemSpawner = new Vector3(randomX, 0.3f, 0f);

        switch (randomItem)
        {
            case 1:
                itemType = ItemType.Item_1;
                break;
            case 2:
                itemType = ItemType.Item_2;
                break;
            case 3:
                itemType = ItemType.Item_3;
                break;
            // case 4:
            //     itemType = ItemType.Item_4;
            //     break;
            // case 5:
            //     itemType = ItemType.Item_5;
            //     break;
        }
        ItemPool.SingletonItemPool.GetItem(itemType, itemSpawner, Quaternion.identity);
    }
}

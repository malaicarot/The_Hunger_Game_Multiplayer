using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum WeaponType
{
    Pistol,
    Rifle,
    Sniper,
    Shotgun,
    Bomb
}
[RequireComponent(typeof(PooledObject))]
public class ItemMovement : PooledObject
{
    float itemDuration = 6f;
    float amplitude = 0.5f;
    float frequency = 2f;
    WeaponType weaponType;
    Vector3 startPosition;

    WeaponSpawn weapon;

    void Start()
    {
        weapon = FindObjectOfType<WeaponSpawn>();
        startPosition = transform.position;
        StartCoroutine(ReturnItems());
    }

    void Update()
    {
        // Tao chuyen dong cho items
        float newY = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector2(startPosition.x, startPosition.y + newY);
    }

    IEnumerator ReturnItems()
    {
        while (true)
        {
            yield return new WaitForSeconds(itemDuration);
            Release();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Transform hand = other.transform.Find("Hand");
            string name = gameObject.name.ToString();
            Release();
            switch (name)
            {
                case "Item_1":
                    weaponType = WeaponType.Pistol;
                    break;
                case "Item_2":
                    weaponType = WeaponType.Rifle;
                    break;
                case "Item_3":
                    weaponType = WeaponType.Sniper;
                    break;
                case "Item_4":
                    weaponType = WeaponType.Bomb;
                    break;
                case "Item_5":
                    weaponType = WeaponType.Shotgun;
                    break;
            }
            if (weaponType != WeaponType.Bomb)
            {
                weapon.EquipWeapon(weaponType, hand);
            }
            else
            {
                BombController bombBag = FindObjectOfType<BombController>();
                bombBag.bombQuantity = 5;
            }
        }
    }
}

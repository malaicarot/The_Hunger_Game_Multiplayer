using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour
{
    Shooting currentWeapon;
    string currentWeaponType;

    float timeExits;
    float weaponDuration = 6f;


    void Start()
    {

    }


    void Update()
    {
        timeExits -= Time.deltaTime;
        if (timeExits <= 0)
        {
            if (currentWeapon != null)
            {
                ReturnWeaponToPool();
            }

            timeExits = 0;
        }
    }

    void ReturnWeaponToPool()
    {
        Debug.Log("Return weapon to pool: " + currentWeapon);
        currentWeapon.Release();
        currentWeapon = null;
        currentWeaponType = "";
    }



    public void EquipWeapon(WeaponType weaponType, Transform targerPosition)
    {
        Debug.Log($"Weapon Params: {weaponType}");

        if (currentWeapon != null && currentWeaponType == weaponType.ToString())
        {
            Debug.Log($"Same weapon: {currentWeaponType} = {weaponType}");
            timeExits = weaponDuration;
        }
        else
        {
            if (currentWeapon != null && currentWeaponType != weaponType.ToString())
            {
                Debug.Log($"Diferrence weapon: {currentWeaponType} != {weaponType}");
                ReturnWeaponToPool();
            }

            Shooting weapon = WeaponPool.SingletonWeaponPool.CreateWeapon(weaponType, targerPosition.position, Quaternion.identity);
            currentWeapon = weapon;
            timeExits = weaponDuration;
            currentWeaponType = weaponType.ToString();
            weapon.transform.SetParent(targerPosition);
            weapon.transform.localPosition = new Vector3(0f, -0.6f, 0f);
            weapon.transform.localRotation = Quaternion.identity;
        }
    }
}

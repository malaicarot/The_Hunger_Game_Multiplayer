using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponSpawn : MonoBehaviourPunCallbacks
{
    GameObject currentWeapon;
    string currentWeaponType;

    Shooting weapon;

    BombController bombController;

    void Start()
    {
        bombController = FindObjectOfType<BombController>();

    }

    void Update()
    {
        if (weapon == null)
        {
            weapon = FindObjectOfType<Shooting>();
        }
    }

    public void DestroyWeapon()
    {
        weapon.DestroyWeapon();
        currentWeapon = null;
        currentWeaponType = "";
    }

    public void EquipWeapon(WeaponType weaponType, Transform targerPosition)
    {
        if (weaponType != WeaponType.Bomb)
        {
            Debug.Log($"Weapon Params: {weaponType}");

            if (currentWeapon != null && currentWeaponType == weaponType.ToString())
            {
                Debug.Log($"Same weapon: {currentWeaponType} = {weaponType}");
                if (weapon != null)
                {
                    weapon.ResetCourtine();
                }
            }
            else
            {
                if (currentWeapon != null && currentWeaponType != weaponType.ToString())
                {
                    Debug.Log($"Diferrence weapon: {currentWeaponType} != {weaponType}");
                    DestroyWeapon();
                }
                SpawnWeapon(weaponType.ToString(), targerPosition);
            }
        }
        else
        {
            bombController.bombQuantity = 5;

        }
    }

    void SpawnWeapon(string type, Transform target)
    {
        GameObject weapon = PhotonNetwork.Instantiate(type, target.position, Quaternion.identity);
        Debug.Log($"weapon: {weapon}");
        weapon.name = type;
        currentWeapon = weapon;
        currentWeaponType = type;
        weapon.transform.SetParent(target);
        weapon.transform.localPosition = new Vector3(0f, -0.6f, 0f);
        weapon.transform.localRotation = Quaternion.identity;
    }
}

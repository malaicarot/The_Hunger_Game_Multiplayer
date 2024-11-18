using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerCollider : MonoBehaviourPunCallbacks
{
    WeaponSpawn weapon;
    WeaponType weaponType;

    void Start()
    {
        weapon = FindObjectOfType<WeaponSpawn>();
    }

    void GetItems(GameObject item, GameObject player, WeaponType _weaponType)
    {
        photonView.RPC("RPC_GetItems", RpcTarget.AllBuffered, item.GetPhotonView().ViewID, player.GetPhotonView().ViewID, _weaponType);
    }

    [PunRPC]
    void RPC_GetItems(int itemId, int playerId, WeaponType _weaponType)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemId);
        PhotonView playerPhotonView = PhotonView.Find(playerId);
        Debug.Log("Player: " + PhotonNetwork.LocalPlayer);

        if (itemPhotonView != null && itemPhotonView.gameObject != null)
        {
            if (itemPhotonView.Owner != PhotonNetwork.LocalPlayer)
            {
                itemPhotonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            if (itemPhotonView.Owner == PhotonNetwork.LocalPlayer)
            {
                PhotonNetwork.Destroy(itemPhotonView.gameObject);
            }

        }
        if (playerPhotonView != null && playerPhotonView.gameObject != null)
        {
            if (playerPhotonView.Owner == PhotonNetwork.LocalPlayer || PhotonNetwork.IsMasterClient)
            {
                Transform hand = playerPhotonView.gameObject.transform.Find("Hand");
                weapon.EquipWeapon(_weaponType, hand);
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            string name = other.gameObject.name.ToString();
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
            GetItems(other.gameObject, gameObject, weaponType);
        }
    }
}

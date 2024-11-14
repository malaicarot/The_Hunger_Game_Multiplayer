using System.Collections;
using Photon.Pun;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Rifle,
    Sniper,
    Shotgun,
    Bomb
}

public class ItemMovement : MonoBehaviourPunCallbacks
{
    [SerializeField] float itemDuration = 6f;
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

    IEnumerator ReturnItems()
    {
        yield return new WaitForSeconds(itemDuration);
        PhotonNetwork.Destroy(gameObject);
    }

    void Update()
    {
        // Tao chuyen dong cho items
        if (PhotonNetwork.IsMasterClient)
        {
            float newY = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector2(startPosition.x, startPosition.y + newY);
        }

    }

    public void GetItems(GameObject item, GameObject player, WeaponType _weaponType)
    {
        photonView.RPC("RPC_GetItems", RpcTarget.AllBuffered, item.GetPhotonView().ViewID, player.GetPhotonView().ViewID, _weaponType);

    }

    [PunRPC]
    void RPC_GetItems(int itemId, int playerId, WeaponType _weaponType)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemId);
        PhotonView playerPhotonView = PhotonView.Find(playerId);

        if (itemPhotonView != null && itemPhotonView.gameObject != null)
        {
            if (itemPhotonView.Owner == PhotonNetwork.LocalPlayer || PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(itemPhotonView.gameObject);
            }
            else
            {
                itemPhotonView.TransferOwnership(PhotonNetwork.MasterClient);
                // PhotonNetwork.Destroy(itemPhotonView.gameObject);
            }
        }

        if (playerPhotonView != null && playerPhotonView.gameObject != null)
        {
            Transform hand = playerPhotonView.gameObject.transform.Find("Hand");
            weapon.EquipWeapon(_weaponType, hand);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            string name = gameObject.name.ToString();
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
            // if (weaponType != WeaponType.Bomb)
            // {
                GetItems(this.gameObject, other.gameObject, weaponType);

                
            // }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BombController : MonoBehaviourPunCallbacks
{
    public int bombQuantity;
    [SerializeField] int maxQuantity = 5;
    void Start()
    {
        bombQuantity = maxQuantity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Throwing();
        }
    }

    void Throwing()
    {
        if (bombQuantity > 0)
        {
            GameObject bomb = PhotonNetwork.Instantiate(WeaponType.Bomb.ToString(), transform.position, Quaternion.identity);
            bomb.name = WeaponType.Bomb.ToString();
            bombQuantity--;
        }
    }

}

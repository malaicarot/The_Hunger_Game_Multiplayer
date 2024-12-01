using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class BombController : MonoBehaviourPun
{
    public int bombQuantity;
    [SerializeField] int maxQuantity = 5;
    [SerializeField] GameObject BombPrefab;
    [SerializeField] Transform throwPoint;
    [SerializeField] TextMeshProUGUI _BombQuantity;
    void Start()
    {
        bombQuantity = maxQuantity;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Throwing();  
            }
            photonView.RPC("RPC_UpdateBombBag", RpcTarget.All, bombQuantity);
        }
    }

    void Throwing()
    {
        if (bombQuantity > 0)
        {
            bombQuantity--;
            photonView.RPC("RPC_ThrowBomb", RpcTarget.All);
            photonView.RPC("RPC_UpdateBombBag", RpcTarget.All, bombQuantity);
        }
    }

    void UpdateBombQuantity()
    {
        _BombQuantity.text = $": {bombQuantity.ToString()}";
    }

    [PunRPC]
    void RPC_ThrowBomb()
    {
        GameObject bomb = Instantiate(BombPrefab, throwPoint.position, Quaternion.identity);
    }

    [PunRPC]
    void RPC_UpdateBombBag(int quantity)
    {
        bombQuantity = quantity;
        UpdateBombQuantity();
    }
}

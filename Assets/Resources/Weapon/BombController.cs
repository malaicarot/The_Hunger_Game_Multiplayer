using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BombController : MonoBehaviourPun
{
    public int bombQuantity;
    [SerializeField] int maxQuantity = 5;
    [SerializeField] GameObject BombPrefab;
    // PhotonView photonView;
    void Start()
    {
        bombQuantity = maxQuantity;
    }

    void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
        {
            Throwing();
        }
    }

    void Throwing()
    {
        if (bombQuantity > 0)
        {
            photonView.RPC("RPC_ThrowBomb", RpcTarget.All);
            
        }
    }

    [PunRPC]
    void RPC_ThrowBomb()
    {
        GameObject bomb = Instantiate(BombPrefab, transform.position, Quaternion.identity);
        bombQuantity--;
    }
}

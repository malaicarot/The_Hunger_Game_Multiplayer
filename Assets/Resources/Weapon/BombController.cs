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
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Q))
        {
            Throwing();
        }
    }

    void Throwing()
    {
        if (bombQuantity > 0)
        {
            photonView.RPC("RPC_ThrowBomb", RpcTarget.AllBuffered);
            bombQuantity--;
        }
    }

    [PunRPC]
    void RPC_ThrowBomb()
    {
        GameObject bomb = PhotonNetwork.Instantiate(WeaponType.Bomb.ToString(), transform.position, Quaternion.identity);
        bomb.name = WeaponType.Bomb.ToString();

        // if(photonView.IsMine){
        //     StartCoroutine(Explosion(bomb));
        // }
    }

    IEnumerator Explosion(GameObject bomb)
    {
        yield return new WaitForSeconds(2f);
        GameObject explode = PhotonNetwork.Instantiate("Explosion", bomb.transform.position, bomb.transform.rotation);
        PhotonNetwork.Destroy(bomb);
        // PhotonNetwork.Destroy(explode);
    }
}

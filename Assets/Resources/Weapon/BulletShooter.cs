using Photon.Pun;
using UnityEngine;
public class BulletShooter : MonoBehaviourPun
{
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float bulletForce = 10.0f;
    Rigidbody2D rb;
    PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
      
    }

    
    void InitializeBullet(Vector2 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (other.transform.position - transform.position).normalized;
            enemyRb.velocity = Vector2.zero;
            enemyRb.AddForce(forceDirection * bulletForce, ForceMode2D.Impulse);
            photonView.RPC("RPC_DestroyBullet", RpcTarget.All);
        }
        else if (other.gameObject.CompareTag("Boundary"))
        {
            photonView.RPC("RPC_DestroyBullet", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_DestroyBullet(){
        Destroy(gameObject);
    }
    // void DestroyBullet()
    // {
    //     if (photonView != null)
    //         if (photonView.IsMine || PhotonNetwork.IsMasterClient)
    //         {
    //             Destroy(gameObject);
    //         }
    //         else
    //         {
    //             Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor MasterClient.");
    //         }

    // }
}

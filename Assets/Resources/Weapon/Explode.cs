using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Explode : MonoBehaviourPunCallbacks
{
    [SerializeField] float explodeForce = 60f;


    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ReturnExplosion());
        }

    }
    IEnumerator ReturnExplosion()
    {
        yield return new WaitForSeconds(0.8f);
        PhotonNetwork.Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D player = other.GetComponent<Rigidbody2D>();
            if (player != null)
            {
                Vector2 forceExplode = (other.transform.position - transform.position).normalized;
                player.AddForce(forceExplode * explodeForce, ForceMode2D.Impulse);
            }
        }
    }
}

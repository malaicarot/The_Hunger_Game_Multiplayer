using Photon.Pun;
using UnityEngine;
public class BulletShooter : MonoBehaviour
{
    [SerializeField] private float bulletForce = 10.0f;
    [SerializeField] LayerMask playerMask;
    [SerializeField] float bulletCheckDistance = 0.2f;
    PhotonView photonView;

    void Start()
    {

        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            Debug.Log("Photon View is Null!");
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         Destroy(gameObject);
    //     }
    //     else if (other.gameObject.CompareTag("Boundary"))
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void FixedUpdate()
    {

        CollisionChecking();


    }

    void CollisionChecking()
    {
        Vector2 raysDirection = -transform.right;
        Debug.DrawRay(transform.position, raysDirection * bulletCheckDistance, Color.red);
        RaycastHit2D bulletHit2D = Physics2D.Raycast(transform.position, raysDirection, bulletCheckDistance, playerMask);
        if(bulletHit2D.collider != null){
            Destroy(gameObject);
            GameObject hitObject = bulletHit2D.collider.gameObject;
            Rigidbody2D enemyRb = hitObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (hitObject.transform.position - transform.position).normalized;
            enemyRb.velocity = Vector2.zero;
            enemyRb.AddForce(forceDirection * bulletForce, ForceMode2D.Impulse);
        }
    }


    [PunRPC]
    void ForceAffected(int playerId)
    {
        Debug.Log("THIS IS PLAYER!!!!!!");
        // Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
        // Vector2 forceDirection = (other.transform.position - transform.position).normalized;
        // enemyRb.velocity = Vector2.zero;
        // enemyRb.AddForce(forceDirection * bulletForce, ForceMode2D.Impulse);
    }
}

using Photon.Pun;
using UnityEngine;
public class BulletShooter : MonoBehaviour
{
    [SerializeField] float bulletForce = 50f;
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

    void Update()
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
}

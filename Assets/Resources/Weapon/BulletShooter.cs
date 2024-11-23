using Photon.Pun;
using UnityEngine;
public class BulletShooter : MonoBehaviour
{
    [SerializeField] private float bulletForce = 10.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (other.transform.position - transform.position).normalized;
            enemyRb.velocity = Vector2.zero;
            enemyRb.AddForce(forceDirection * bulletForce, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}

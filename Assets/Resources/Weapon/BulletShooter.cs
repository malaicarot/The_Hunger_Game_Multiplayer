using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
public class BulletShooter : MonoBehaviourPun
{
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float bulletForce = 10.0f;
    Rigidbody2D rb;
    Shooting direciton;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        direciton = FindObjectOfType<Shooting>();

        
        Vector2 _direction = -direciton._FirePoint.right;
        InitializeBullet(_direction);
    }

    public void InitializeBullet(Vector2 direction)
    {
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            Debug.Log(gameObject);
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
            PhotonNetwork.Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Boundary"))
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}

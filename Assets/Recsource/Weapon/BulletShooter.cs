using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PooledObject))]
public class BulletShooter : PooledObject
{
    [SerializeField] private float speed = 20.0f;
    [SerializeField] private float bulletForce = 10.0f;
    Rigidbody2D rb;

    Controller player;

    void Awake(){
        player = FindObjectOfType<Controller>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void InitializeBullet(Vector2 direction){
        if(rb != null){
            rb.velocity = direction * speed;

        }else{
            Debug.Log(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Enemy")){
            Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = other.transform.position - transform.position;
            enemyRb.AddForce(forceDirection * bulletForce, ForceMode2D.Impulse);
            Release();
        }
    }
}

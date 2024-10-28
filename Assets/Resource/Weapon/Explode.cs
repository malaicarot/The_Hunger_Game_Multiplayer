using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class Explode : PooledObject
{
    [SerializeField] float explodeForce = 60f;

    public void EndExplode()
    {
        StartCoroutine(ReturnExplosion());
    }
    IEnumerator ReturnExplosion()
    {
        yield return new WaitForSeconds(0.8f);
        Release();
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

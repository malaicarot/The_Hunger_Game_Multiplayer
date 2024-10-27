using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PooledObject))]
public class ThrowGrenade : PooledObject
{
    // [SerializeField] int bombQuantity = 5;
    [SerializeField] float bounceForce = 10f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        BombBounce();
    }
    void Update()
    {
        // float fire = Input.GetAxis("Fire1");
        // if (Input.GetButtonDown("Fire1") && fire != 0 && bombQuantity > 0)
        // {
        //     Throwing();
        //     bombQuantity--;
        // }
    }

    void BombBounce(){
        rb.AddForce(Vector2.right * bounceForce);
        

    }
}

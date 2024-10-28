using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] public int bombQuantity;
    [SerializeField] int maxQuantity = 5;
    void Start()
    {
        bombQuantity = maxQuantity;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Throwing();
        }
    }

    void Throwing()
    {
        if (bombQuantity > 0)
        {
            WeaponPool.SingletonWeaponPool.CreateWeapon(WeaponType.Bomb, transform.position, Quaternion.identity);
            bombQuantity--;

        }else{
            // Debug.Log("Out of bombs!");
        }
    }

}

using System.Collections;
using Photon.Pun;
using UnityEngine;

public enum BulletType
{
    Bullet_Pistol,
    Bullet_Rifle,
    Bullet_Sniper,
    Bullet_Shotgun,
}

public class Shooting : MonoBehaviourPunCallbacks
{
    [SerializeField] float timeExits = 10f;
    public string gunType;

    void Start()
    {
        if (gameObject.name.ToString() != "Bomb")
        {
            StartCoroutine(timeExitsWeapon());
        }

    }
    void Update()
    {
        // if (bulletShooter == null)
        // {
        //     bulletShooter = FindObjectOfType<BulletShooter>();
        // }

        // if (gameObject.name.ToString() != "Bomb")
        // {
        //     float fire = Input.GetAxis("Fire1");
        //     if (photonView.IsMine && Input.GetButtonDown("Fire1") && fire != 0)
        //     {
        //         // Shoot();
        //         Debug.Log("pew pew pew!");
        //     }
        // }
        // else
        // {
        //     if (photonView.IsMine)
        //     {
        //         StartCoroutine(Explosion());
        //     }
        // }
    }


    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Instantiate("Explosion", transform.position, transform.rotation);
        PhotonNetwork.Destroy(gameObject);
    }

    IEnumerator timeExitsWeapon()
    {
        yield return new WaitForSeconds(timeExits);
        Destroy(gameObject);

    }

}

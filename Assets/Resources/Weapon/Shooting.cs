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
    BulletType type;
    [SerializeField] Transform firePoint;
    [SerializeField] float timeExits = 10f;
    public Transform _FirePoint
    {
        get { return firePoint; }
    }
    BulletShooter bulletShooter;
    Coroutine weaponExitsCoroutine;

    void Start()
    {
        if (gameObject.name.ToString() != "Bomb")
        {
            StartWeaponCoroutine();
        }

    }
    void Update()
    {
        if (bulletShooter == null)
        {
            bulletShooter = FindObjectOfType<BulletShooter>();
        }

        if (gameObject.name.ToString() != "Bomb")
        {
            float fire = Input.GetAxis("Fire1");
            if (photonView.IsMine && Input.GetButtonDown("Fire1") && fire != 0)
            {
                Shoot();
            }
        }
        else
        {
            if (photonView.IsMine)
            {
                StartCoroutine(Explosion());
            }
        }
    }
    

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Instantiate("Explosion", transform.position, transform.rotation);
        PhotonNetwork.Destroy(gameObject);
    }

    void StartWeaponCoroutine()
    {
        if (weaponExitsCoroutine != null)
        {
            StopCoroutine(weaponExitsCoroutine);
        }
        weaponExitsCoroutine = StartCoroutine(timeExitsWeapon());
    }

    IEnumerator timeExitsWeapon()
    {
        yield return new WaitForSeconds(timeExits);
        DestroyWeapon();
    }

    public void ResetCourtine()
    {
        StartWeaponCoroutine();
    }

    public void DestroyWeapon()
    {
        PhotonNetwork.Destroy(gameObject);
    }


    void GetBullet(GameObject weapon, BulletType bulletType)
    {
        photonView.RPC("RPC_GetBullet", RpcTarget.AllBuffered, weapon.GetPhotonView().ViewID, bulletType);
    }

    [PunRPC]
    void RPC_GetBullet(int weaponId, BulletType _bulletType)
    {
        PhotonView weaponPhotonView = PhotonView.Find(weaponId);
        if (weaponPhotonView != null && weaponPhotonView.gameObject != null)
        {
            if (weaponPhotonView.Owner == PhotonNetwork.LocalPlayer || PhotonNetwork.IsMasterClient)
            {
                GameObject bullet = PhotonNetwork.Instantiate(type.ToString(), firePoint.position, Quaternion.identity);
                if (bullet != null)
                {
                    bullet.name = type.ToString();
                }
            }
        }
    }

    void Shoot()
    {
        switch (gameObject.name)
        {
            case "Pistol":
                type = BulletType.Bullet_Pistol;
                break;
            case "Rifle":
                type = BulletType.Bullet_Rifle;
                break;
            case "Sniper":
                type = BulletType.Bullet_Sniper;
                break;
            case "Shotgun":
                type = BulletType.Bullet_Shotgun;
                break;
        }
        GetBullet(this.gameObject, type);
    }
}

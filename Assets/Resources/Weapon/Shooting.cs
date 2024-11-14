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
        StartWeaponCoroutine();
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
            if (Input.GetButtonDown("Fire1") && fire != 0)
            {
                Shoot();
            }
        }
        else
        {
            StartCoroutine(Explosion());
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

        GameObject bullet = PhotonNetwork.Instantiate(type.ToString(), firePoint.position, Quaternion.identity);
        if (bullet != null)
        {
            bullet.name = type.ToString();
        }
    }
}

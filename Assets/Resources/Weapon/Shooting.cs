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
    [SerializeField] GameObject ExplosionPrefab;
    public string gunType;

    void Start()
    {
        if (gunType != "Bomb")
        {
            StartCoroutine(timeExitsWeapon());
        }else{
            StartCoroutine(Explosion());
        }

    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2f);
        Instantiate(ExplosionPrefab, transform.position, transform.rotation);
        SoundController._instance.ExplodeAudioPlay();
        Destroy(gameObject);
    }

    IEnumerator timeExitsWeapon()
    {
        yield return new WaitForSeconds(timeExits);
        Destroy(gameObject);
    }

}

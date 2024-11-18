using System.Collections;
using Photon.Pun;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Rifle,
    Sniper,
    Shotgun,
    Bomb
}

public class ItemController : MonoBehaviourPun
{
    [SerializeField] float itemDuration = 6f;
    float amplitude = 0.5f;
    float frequency = 2f;
    Vector3 startPosition;

    WeaponType weaponType;

    void Start()
    {
        startPosition = transform.position;
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(ReturnItems());
        }
    }

    IEnumerator ReturnItems()
    {
        yield return new WaitForSeconds(itemDuration);
        PhotonNetwork.Destroy(gameObject);
    }

    void Update()
    {
        // Tao chuyen dong cho items
        if (PhotonNetwork.IsMasterClient)
        {
            float newY = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector2(startPosition.x, startPosition.y + newY);
        }
    }

}

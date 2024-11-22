using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float fallSpeed = -10f;

    [SerializeField] float checkDistance = 1f;
    [SerializeField] LayerMask groundMask;


    [SerializeField] GameObject[] WeaponPrefabs;
    [SerializeField] GameObject[] BulletPrefab;

    [SerializeField] Transform firePoint;
    public Transform _FirePoint{
        get{return firePoint;}
    }



    Rigidbody2D rb;
    public Rigidbody2D Rb { get; }
    Animator animator;
    bool isMoving = false;
    bool isGrounded = true;
    bool isDown = false;
    bool isDeath = false;
    /***************************************************/
    MapController map;
    /***************************************************/
    Vector2 moveInput;
    Vector2 moveVelocity;

    Quaternion playerRotation = Quaternion.Euler(0, 360, 0);

    PhotonView photonView;
    Shooting myGun;


    /***************************/
    WeaponType weaponType;
    GameObject currentWeapon;
    string currentWeaponType;
    int weaponIndex;


    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        map = FindObjectOfType<MapController>();

    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if(myGun == null){
                myGun = FindObjectOfType<Shooting>();
            }

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            float jumpInput = Input.GetAxisRaw("Jump");
            /***************************************************/
            isMoving = moveHorizontal != 0;
            isDown = moveVertical != 0;
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isDeath", isDeath);
            /***************************************************/
            moveInput = new Vector2(moveHorizontal, 0);
            moveVelocity = moveInput * speed;
            /***************************************************/

            if (moveInput.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (moveInput.x < 0)
            {
                transform.rotation = playerRotation;
            }
            /***************************************************/
            if (jumpInput != 0 && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            float fire = Input.GetAxis("Fire1");
            if (Input.GetButtonDown("Fire1") && fire != 0 && myGun != null)
            {
                // myGun.Shoot(myGun.gunType);
                // Transform muzzle = gameObject.transform.Find("Muzzle");
                photonView.RPC("RPC_GetBullet", RpcTarget.All, myGun.gunType, _FirePoint.gameObject.GetComponent<PhotonView>().ViewID);
            }
        }

    }

    void GroundChecking()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundMask);
        isGrounded = hit2D.collider != null;
        Debug.DrawRay(transform.position, Vector2.down * checkDistance, Color.red);
    }

    void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            GroundChecking();
            if (isMoving)
            {
                rb.velocity = new Vector2(moveVelocity.x, rb.velocity.y);
            }
            /***************************************************/

            if (isDown)
            {
                map.SettingSurfaceArc();
                map.ActiveResetSurfaceArc();
            }
            /***************************************************/

            if (rb.velocity.y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, fallSpeed));
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            isDeath = true;
            // Destroy(gameObject);
        }
        else if (other.CompareTag("Item"))
        {
            ItemController item = other.gameObject.GetComponent<ItemController>();
            string name = item.ItemType.ToString();
            switch (name)
            {
                case "Item_1":
                    weaponType = WeaponType.Pistol;
                    break;
                case "Item_2":
                    weaponType = WeaponType.Rifle;
                    break;
                case "Item_3":
                    weaponType = WeaponType.Sniper;
                    break;
                case "Item_4":
                    weaponType = WeaponType.Bomb;
                    break;
                case "Item_5":
                    weaponType = WeaponType.Shotgun;
                    break;
            }

            if (photonView.IsMine)
            {
                EquipWeapon(weaponType);
                photonView.RPC("RPC_DestroyItem", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
            }

        }

    }

    [PunRPC]
    void RPC_DestroyItem(int itemId)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemId);
        if (itemPhotonView != null && itemPhotonView.gameObject != null)
        {
            Destroy(itemPhotonView.gameObject);
        }
    }

    void EquipWeapon(WeaponType weaponType)
    {
        if (weaponType != WeaponType.Bomb)
        {
            if (currentWeapon != null)
            {
                Destroy(currentWeapon);
            }
            currentWeapon = null;
            currentWeaponType = "";

            photonView.RPC("RPC_SpawnWeapon", RpcTarget.All, weaponType, gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    void RPC_SpawnWeapon(WeaponType type, int playerID)
    {
        PhotonView playerPhotonView = PhotonView.Find(playerID);
        Transform target = playerPhotonView.gameObject.transform;
        switch (type)
        {

            case WeaponType.Pistol:
                weaponIndex = 0;
                break;
            case WeaponType.Rifle:
                weaponIndex = 1;
                break;
            case WeaponType.Shotgun:
                weaponIndex = 2;
                break;
            case WeaponType.Sniper:
                weaponIndex = 3;
                break;
        }
        GameObject weapon = Instantiate(WeaponPrefabs[weaponIndex], target.position, Quaternion.identity);
        currentWeapon = weapon;
        currentWeaponType = type.ToString();
        weapon.transform.SetParent(target);
        weapon.transform.localPosition = new Vector3(0f, -0.6f, 0f);
        weapon.transform.localRotation = Quaternion.identity;
    }


    [PunRPC]
    void RPC_GetBullet(string type, int muzzleID){
        PhotonView muzzlePhotonView = PhotonView.Find(muzzleID);
        if(muzzlePhotonView != null && muzzlePhotonView.gameObject != null){
            Instantiate(BulletPrefab[0], muzzlePhotonView.transform.position, Quaternion.identity);
        }
    }
}

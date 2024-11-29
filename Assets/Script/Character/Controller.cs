using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Controller : MonoBehaviour
{

    [SerializeField] GameObject bodyGameObject;
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float fallSpeed = -10f;
    [SerializeField] float checkDistance = 1f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] GameObject[] WeaponPrefabs;
    [SerializeField] GameObject[] BulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject ResultUI;
    [SerializeField] GameObject BombBag;
    float amplitude = 0.5f;
    [SerializeField] float frequency = -20f;
    Vector3 PlayerRotation;

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
    BombController bombBag;
    /***************************/

    WeaponType weaponType;
    GameObject currentWeapon;
    string currentWeaponType;
    int weaponIndex;
    int bulletIndex;

    float fireRateCountdown;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        animator = bodyGameObject.GetComponent<Animator>();
        map = FindObjectOfType<MapController>();
        bombBag = GetComponent<BombController>();
        if (photonView.IsMine)
        {
            photonView.RPC("RPC_HideReSultPanel", RpcTarget.All);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (myGun == null)
            {
                myGun = GetComponentInChildren<Shooting>();
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
                bodyGameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                PlayerRotation = Vector2.right;
            }
            else if (moveInput.x < 0)
            {
                bodyGameObject.transform.rotation = playerRotation;
                PlayerRotation = Vector2.left;
            }
            /***************************************************/
            if (jumpInput != 0 && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
             

            if(fireRateCountdown > 0){
                fireRateCountdown -= Time.deltaTime;
            }

            if (Input.GetButtonDown("Fire1") && myGun != null && fireRateCountdown <= 0)
            {
                Vector3 muzzle = firePoint.position;
                Quaternion bulletRotation = firePoint.rotation;
                photonView.RPC("RPC_GetBullet", RpcTarget.All, myGun.gunType, muzzle, bulletRotation, PlayerRotation * myGun.bulletSpeed);
                fireRateCountdown = myGun.GetFireRate();
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


    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            GameRoomManager gameManager = FindObjectOfType<GameRoomManager>();
            isDeath = true;
            gameManager.PlayerFallOut(photonView.OwnerActorNr);
            StartCoroutine(DestroyPlayer());
        }
        else if (other.CompareTag("Item"))
        {
            ItemController item = other.gameObject.GetComponent<ItemController>();
            string name = item.ItemType.ToString();
            SoundController._instance.GetItemAudioPlay();
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
                    bombBag.bombQuantity = 5;
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

    public void ActiveReSultPanel()
    {
        photonView.RPC("RPC_ActiveReSultPanel", RpcTarget.All);
    }

    [PunRPC]
    void RPC_ActiveReSultPanel()
    {
        BombBag.SetActive(false);
        StartCoroutine(AppearMedal());
    }

    IEnumerator AppearMedal(){
        yield return new WaitForSeconds(2f);
        ResultUI.SetActive(true);
        float newY = Mathf.Sin(frequency) * amplitude;
        ResultUI.transform.position = new Vector2(ResultUI.transform.position.x, ResultUI.transform.position.y + newY);
    }

    [PunRPC]
    void RPC_HideReSultPanel()
    {
        ResultUI.SetActive(false);
    }
    
    [PunRPC]
    public void RPC_DestroyItem(int itemId)
    {
        PhotonView itemPhotonView = PhotonView.Find(itemId);
        if (itemPhotonView != null && itemPhotonView.gameObject != null)
        {
            Destroy(itemPhotonView.gameObject);
        }
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        if (weaponType != WeaponType.Bomb)
        {
            photonView.RPC("RPC_SpawnWeapon", RpcTarget.All, weaponType, gameObject.GetComponent<PhotonView>().ViewID);

        }
    }

    [PunRPC]
    public void RPC_SpawnWeapon(WeaponType type, int playerID)
    {
        Transform target = bodyGameObject.transform;
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
            currentWeaponType = "";
        }
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
        fireRateCountdown = 0;
    }

    [PunRPC]
    public void RPC_GetBullet(string type, Vector3 position, Quaternion bulletRotation, Vector3 velocity)
    {
        switch (type)
        {
            case "Pistol":
                bulletIndex = 0;
                break;
            case "Rifle":
                bulletIndex = 1;
                break;
            case "Shotgun":
                bulletIndex = 2;
                break;
            case "Sniper":
                bulletIndex = 3;
                break;
        }
        GameObject bullet = Instantiate(BulletPrefab[bulletIndex], position, bulletRotation);
        SoundController._instance.ShootAudioPlay(type);
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
        Destroy(bullet, 4f);
    }
}

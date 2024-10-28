using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float fallSpeed = -10f;

    [SerializeField] float checkDistance = 1f;
    [SerializeField] LayerMask groundMask;

    Rigidbody2D rb;
    Animator animator;
    bool isMoving = false;
    bool isGrounded = true;

    /***************************************************/
    bool isDown = false;
    // PlatformEffector2D secondFloor;
    MapController map;
    /***************************************************/
    Vector2 moveInput;
    Vector2 moveVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        map = FindObjectOfType<MapController>();
        // GameObject Floor = GameObject.Find("SecondFloor");
        // secondFloor = Floor.GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float jumpInput = Input.GetAxisRaw("Jump");
        /***************************************************/
        isMoving = moveHorizontal != 0;
        isDown = moveVertical != 0;
        animator.SetBool("isMoving", isMoving);
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
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        /***************************************************/
        if (jumpInput != 0 && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Boundary")){
            Debug.Log("Die");
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;


    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float ceilingCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    private bool isGrounded;
    private bool playerUnlocked;
    private bool wallDetected;
    private bool ceilingDetected;
    [HideInInspector] public bool ledgeDetected;


    [Header("Slide info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideCoolDown;
    private float slideTimeCounter;
    private bool isSliding;
    private float slideCoolDownCounter;

    [Header("Ledge info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    private Vector2 climbBeginPos;
    private Vector2 climbOverPos;
    private bool canGrab = true;
    private bool canClimb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllers();
        slideTimeCounter -= Time.deltaTime;
        slideCoolDownCounter -= Time.deltaTime;

        if (playerUnlocked) {
            Movement(); 
        }

        if (isGrounded) canDoubleJump = true;
        
        CheckCollisions();
        CheckInput();
        CheckForSlide();
        CheckForLedge();
    }

    void Movement() {
        if (wallDetected) return;
        if(isSliding) {
            rb.velocity = new Vector2(slideSpeed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    void CheckInput() {
        if (Input.GetButtonDown("Horizontal")) playerUnlocked = true;

        if(Input.GetKeyDown(KeyCode.Space)) {
            JumpButton();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            slideButton();
        }
    }

    void JumpButton() {
        if (isSliding) return;
        if (isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if (canDoubleJump) {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    void slideButton() {
        if (rb.velocity.x != 0 && slideCoolDownCounter < 0) {
            isSliding = true;
            slideTimeCounter = slideTime;
            slideCoolDownCounter = slideCoolDown;
        }
    }

    void CheckCollisions() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
        ceilingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceilingCheckDistance, whatIsGround);
    }

    void CheckForSlide() {
        if(slideTimeCounter < 0 && !ceilingDetected) {
            isSliding = false;
        }
    }

    void CheckForLedge() {
        if (ledgeDetected && canGrab) {
            canGrab = false;
            Vector2 ledgePos = GetComponentInChildren<ledgeDetection>().transform.position;
            climbBeginPos = ledgePos + offset1;
            climbOverPos = ledgePos + offset2;
            canClimb = true;
        }

        if (canClimb) {
            transform.position = climbBeginPos;
        }
    }

    void LedgeClimbOver() {
        canClimb = false;
        transform.position = climbOverPos;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrab = true;

    void AnimatorControllers() {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - ceilingCheckDistance));
    }
}

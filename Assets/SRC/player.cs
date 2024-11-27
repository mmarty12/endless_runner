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
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    private bool isGrounded;
    private bool playerUnlocked;
    private bool wallDetected;


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

        if (playerUnlocked && !wallDetected) {
            Movement() 
        }

        if (isGrounded) canDoubleJump = true;
        
        CheckCollisions();
        CheckInput();
    }

    void Movement() {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void CheckInput() {
        if (Input.GetButtonDown("Horizontal")) playerUnlocked = true;

        if(Input.GetKeyDown(KeyCode.Space)) {
            JumpButton();
        }
    }

    void JumpButton() {
        if (isGrounded) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        } else if (canDoubleJump) {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
        }
    }

    void CheckCollisions() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, Vector2.zero, 0, whatIsGround);
    }

    void AnimatorControllers() {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}

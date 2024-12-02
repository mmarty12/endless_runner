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
    private SpriteRenderer sr;

    private bool isDead;

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

    [Header("Speed info")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float milestoneIncrease;
    private float milestone;
    private float defaultMilestoneIncrease;
    private float defaultSpeed;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        milestone = milestoneIncrease;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncrease = milestoneIncrease;
    }

    void Update()
    {
        AnimatorControllers();
        slideTimeCounter -= Time.deltaTime;
        slideCoolDownCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.K)) {
            Knockback();
        }

        if (Input.GetKeyDown(KeyCode.L) && !isDead) {
            StartCoroutine(Death());
        }

        if (isDead) return;

        if (isKnocked) return;

        if (playerUnlocked) {
            Movement(); 
        }

        if (isGrounded) canDoubleJump = true;

        SpeedController();
        
        CheckCollisions();
        CheckInput();
        CheckForSlide();
        CheckForLedge();
    }

    void Movement() {
        if (wallDetected) {
            SpeedReset();
            return;
        }

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
            rb.gravityScale = 0;
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
        rb.gravityScale = 5;
        transform.position = climbOverPos;
        Invoke("AllowLedgeGrab", .1f);
    }

    private void AllowLedgeGrab() => canGrab = true;

    void SpeedController() {
        if (moveSpeed == maxSpeed) return;

        if (transform.position.x > milestone) {
            milestone += milestoneIncrease;
            moveSpeed *= speedMultiplier;
            milestoneIncrease *= speedMultiplier;

            if (moveSpeed > maxSpeed) {
                moveSpeed = maxSpeed;
            }
        }
    }

    void SpeedReset() {
        moveSpeed = defaultSpeed;
        milestoneIncrease = defaultMilestoneIncrease;
    }

    void RollAnimFinished() => anim.SetBool("canRoll", false);

    private IEnumerator Invincibility() {
        Color originalColor = sr.color;
        Color darkenColor = new Color(originalColor.r, originalColor.g, originalColor.b, .5f);

        canBeKnocked = false;
        for (int i = 0; i < 5; i++) {
            sr.color = darkenColor;
            yield return new WaitForSeconds(.2f);

            sr.color = originalColor;
            yield return new WaitForSeconds(.2f);
        }
        canBeKnocked = true;
    }

    void Knockback() {
        if (!canBeKnocked) return;
        StartCoroutine(Invincibility());
        isKnocked = true;
        rb.velocity = knockbackDir;
    }

    void CancelKnockback() => isKnocked = false;

    IEnumerator Death() {
        isDead = true;
        canBeKnocked = false;
        rb.velocity = knockbackDir;
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(.5f);
        rb.velocity = new Vector2(0,0);
    }

    void AnimatorControllers() {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);
        anim.SetBool("isKnocked", isKnocked);

        if (rb.velocity.y < -20) {
            anim.SetBool("canRoll", true);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - ceilingCheckDistance));
    }
}

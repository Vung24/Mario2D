using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private int maxJump = 2;

    private int jumpCount = 0;
    private bool isGround;
    private Animator animator;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private AudioManager audioManager;
    private Collider2D myCollider;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
        myCollider = FindAnyObjectByType<Collider2D>();
    }

    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
            audioManager.PlayJumpSound();
        }

        // ✅ FIX: Kiểm tra đứng trên Brick + Ground
        bool wasGrounded = isGround;
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // THÊM: Kiểm tra đứng trên Brick (nếu Brick không trong groundLayer)
        if (!isGround)
        {
            isGround = IsStandingOnBrick();
        }

        if (isGround && !wasGrounded)
        {
            jumpCount = 0;
        }
    }
    private bool IsStandingOnBrick()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, 0.2f);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Brick"))
            {
                return true;  // Đứng trên Brick → coi như Ground!
            }
        }
        return false;
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        bool isJumpping = jumpCount > 0 || !isGround;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumpping", isJumpping);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            if (this.transform.position.y < collision.transform.position.y)
            {
                Brick brick = collision.gameObject.GetComponent<Brick>();
                if (brick != null)
                {
                    brick?.OnHitBrick();
                }
            }
        }
    }
}
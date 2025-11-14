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

    // --- tùy chọn: dùng layer hoặc tag để nhận diện brick
    [SerializeField] private LayerMask brickLayer; // set layer cho bricks trong Inspector
    [SerializeField] private string brickTag = "Brick"; // hoặc dùng tag

    private int jumpCount = 0;
    private bool isGround;
    private Animator animator;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private AudioManager audioManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;
        HandleMovement();
        HandleJump();
        UpdateAnimation();
    }

    private void HandleMovement()
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

        bool wasGrounded = isGround;
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGround && !wasGrounded)
        {
            jumpCount = 0;
        }
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.velocity.x) > 0.1f;
        bool isJumpping = jumpCount > 0 || !isGround;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumpping", isJumpping);
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isBrick = false;
        if (!string.IsNullOrEmpty(brickTag) && collision.gameObject.CompareTag(brickTag))
            isBrick = true;
        else if (((1 << collision.gameObject.layer) & brickLayer) != 0)
            isBrick = true;

        if (!isBrick) return;

        // Duyệt contact points để tìm điểm va chạm phía trên đầu player
        foreach (ContactPoint2D cp in collision.contacts)
        {
            // cp.point là world position của điểm chạm
            // so sánh với vị trí player (có thể dùng transform.position.y hoặc một head offset)
            float playerHeadY = transform.position.y + (GetComponent<Collider2D>().bounds.extents.y * 0.6f); // khoảng đầu
            if (cp.point.y > playerHeadY)
            {
                // Va chạm vào phần trên của player (đập gạch từ dưới)
                // Gọi Brick.Hit() nếu có, hoặc trigger Animator trực tiếp
                Brick brick = collision.collider.GetComponent<Brick>();
                if (brick != null)
                {
                    brick.Hit();
                }
                else
                {
                    Animator a = collision.collider.GetComponent<Animator>();
                    if (a != null)
                    {
                        a.SetTrigger("Hit");
                    }
                }

                // Có thể break nếu chỉ cần xử lý 1 lần
                break;
            }
        }
    } */
}
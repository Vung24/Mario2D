using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; //Các biến này không thể truy cập từ script khác, nhưng vẫn chỉnh được trong Inspector.
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

    private void Awake()
    {
  
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
}

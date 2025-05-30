using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float crouchSpeed = 2f;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Eğilme Ayarları")]
    public Collider2D standingCollider;
    public Collider2D crouchingCollider;
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isCrouching;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Crouch(bool state)
    {
        isCrouching = state;
        if (standingCollider != null && crouchingCollider != null)
        {
            standingCollider.enabled = !state;
            crouchingCollider.enabled = state;
        }
    }
}

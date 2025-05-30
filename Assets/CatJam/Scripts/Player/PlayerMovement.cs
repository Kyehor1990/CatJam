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

    [Header("Dodge Ayarları")]
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;
    public float dodgeCooldown = 1f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isCrouching;
    private bool isDodging;
    private float dodgeTimer = 0f;
    private float lastDodgeTime = -Mathf.Infinity;
    private Vector2 dodgeDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isDodging)
            moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.LeftControl))
            Crouch(true);
        else
            Crouch(false);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && !isDodging)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);


        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDodgeTime + dodgeCooldown && !isDodging)
        {
            StartDodge();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isDodging)
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;
            dodgeTimer -= Time.fixedDeltaTime;
            if (dodgeTimer <= 0)
                EndDodge();
        }
        else
        {
            float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
            rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        }
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

    void StartDodge()
    {
        isDodging = true;
        dodgeTimer = dodgeDuration;
        lastDodgeTime = Time.time;
        dodgeDirection = new Vector2(transform.localScale.x, 0).normalized;
    }

    void EndDodge()
    {
        isDodging = false;
    }

    public bool IsDodging()
    {
        return isDodging;
    }
}

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float crouchSpeed = 2f;
    public Transform target;

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

    [Header("Wall Bounce Ayarları")]
    public float wallBounceForceX = 12f;
    public float wallBounceForceY = 16f;
    public float wallBounceLockTime = 0.2f;

    private Rigidbody2D rb;
    public bool isGrounded;
    private float moveInput;
    private bool isCrouching;
    public bool isDodging;
    private float dodgeTimer = 0f;
    private float lastDodgeTime = -Mathf.Infinity;
    private Vector2 dodgeDirection;
    private bool isWallBouncing = false;
    private float wallBounceInputLockTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (target != null)
        {
            float scaleX = transform.localScale.x;
            if (target.position.x > transform.position.x)
                scaleX = Mathf.Abs(scaleX);
            else
                scaleX = Mathf.Abs(scaleX) * -1;

            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }

        if (!isDodging && !isWallBouncing)
        {
            moveInput = wallBounceInputLockTimer <= 0 ? Input.GetAxisRaw("Horizontal") : 0;
        }

        Crouch(Input.GetKey(KeyCode.LeftControl));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isCrouching && !isDodging)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDodgeTime + dodgeCooldown && !isDodging && isGrounded)
        {
            StartDodge();
        }

        if (wallBounceInputLockTimer > 0)
            wallBounceInputLockTimer -= Time.deltaTime;
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
        else if (!isWallBouncing)
        {
            float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
            rb.linearVelocity = new Vector2(moveInput * currentSpeed, rb.linearVelocity.y);
        }
    }

    void Crouch(bool state)
    {
        isCrouching = state;
        if (standingCollider && crouchingCollider)
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

        float dodgeDirX = moveInput != 0 ? Mathf.Sign(moveInput) : Mathf.Sign(transform.localScale.x);
        dodgeDirection = new Vector2(dodgeDirX, 0).normalized;
    }

    void EndDodge()
    {
        isDodging = false;
    }

    public bool IsDodging()
    {
        return isDodging;
    }

    void WallBounce(int direction)
    {
        isWallBouncing = true;

        // Bu sekme hareketi oyuncuyu çapraz yukarı fırlatır
        Vector2 bounceVelocity = new Vector2(wallBounceForceX * direction, wallBounceForceY);
        rb.linearVelocity = bounceVelocity;

        wallBounceInputLockTimer = wallBounceLockTime;
        Invoke(nameof(ResetWallBounce), wallBounceLockTime);
    }

    void ResetWallBounce()
    {
        isWallBouncing = false;
    }

    // Duvardan sekme tetikleyici
    void OnTriggerEnter2D(Collider2D other)
    {
        WallBounceTrigger trigger = other.GetComponent<WallBounceTrigger>();
        if (trigger != null && !isGrounded)
        {
            WallBounce(trigger.bounceDirection);
        }
    }
}

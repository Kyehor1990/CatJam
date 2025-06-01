using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public enum AttackPattern
    {
        JumpOnPlayer,
        StretchArm,
        SlimeProjectile
    }

    [Header("Player & Movement")]
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public float patternDelay = 2f;

    [Header("Prefabs")]
    public GameObject stretchArmPrefab;
    public GameObject lowProjectilePrefab;
    public GameObject midProjectilePrefab;
    public GameObject highProjectilePrefab;

    [Header("Projectile Spawn Points")]
    public Transform lowProjectileSpawnPoint;
    public Transform midProjectileSpawnPoint;
    public Transform highProjectileSpawnPoint;

    [SerializeField] private float projectileDestroyTime = 3f;

    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float pushValue = 5f; // Adjust the push value as needed

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(PatternLoop());
    }

    void Update()
    {
        if (!isAttacking && Vector2.Distance(transform.position, player.position) > attackRange)
        {
            FollowPlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        FlipTowardsPlayer();
    }

    void FollowPlayer()
    {
        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void FlipTowardsPlayer()
    {
        if (player != null)
        {
            Vector3 scale = transform.localScale;
            scale.x = player.position.x > transform.position.x ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    IEnumerator PatternLoop()
    {
        while (true)
        {
            if (!isAttacking && Vector2.Distance(transform.position, player.position) <= attackRange)
            {
                isAttacking = true;
                AttackPattern pattern = ChooseRandomPattern();
                yield return ExecutePattern(pattern);
                isAttacking = false;
            }

            yield return new WaitForSeconds(patternDelay);
        }
    }

    AttackPattern ChooseRandomPattern()
    {
        int count = System.Enum.GetValues(typeof(AttackPattern)).Length;
        return (AttackPattern)Random.Range(0, count);
    }

    IEnumerator ExecutePattern(AttackPattern pattern)
    {
        switch (pattern)
        {
            case AttackPattern.JumpOnPlayer:
                yield return JumpOnPlayer();
                break;
            case AttackPattern.StretchArm:
                yield return StretchArm();
                break;
            case AttackPattern.SlimeProjectile:
                yield return SlimeProjectileAttack();
                break;
        }
    }

    IEnumerator JumpOnPlayer()
    {
        Debug.Log("Jump Attack Started");

        Vector2 jumpDir = new Vector2(player.position.x - transform.position.x, 0).normalized;
        float jumpForceX = 8f;
        float jumpHeight = 4f;
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float jumpForceY = Mathf.Sqrt(2 * jumpHeight * gravity);

        rb.velocity = new Vector2(jumpDir.x * jumpForceX, jumpForceY);

        yield return new WaitForSeconds(0.8f);
        float timeout = 2f;
        float timer = 0f;

        while (!IsGrounded() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Jump Attack Ended");
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down * 0.1f, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
        return hit.collider != null;
    }

    IEnumerator StretchArm()
    {
        Debug.Log("Stretch Arm Attack Started");
        yield return new WaitForSeconds(0.2f);

        if (stretchArmPrefab != null && midProjectileSpawnPoint != null)
        {
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            Quaternion rotation = direction > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

            GameObject arm = Instantiate(stretchArmPrefab, midProjectileSpawnPoint.position, rotation);
            arm.transform.parent = transform;

            yield return new WaitForSeconds(2f);
            Destroy(arm);
        }

        yield return new WaitForSeconds(1f);
        Debug.Log("Stretch Arm Attack Ended");
    }

    IEnumerator SlimeProjectileAttack()
    {
        Debug.Log("Slime Projectile Attack Started");
        yield return new WaitForSeconds(0.2f);

        Vector2 dir = (player.position - transform.position).normalized;
        int randomIndex = Random.Range(0, 3);

        GameObject selectedPrefab = null;
        Transform spawnPoint = null;
        float speed = 5f;

        switch (randomIndex)
        {
            case 0:
                selectedPrefab = lowProjectilePrefab;
                spawnPoint = lowProjectileSpawnPoint;
                speed = 5f;
                break;
            case 1:
                selectedPrefab = midProjectilePrefab;
                spawnPoint = midProjectileSpawnPoint;
                speed = 6f;
                break;
            case 2:
                selectedPrefab = highProjectilePrefab;
                spawnPoint = highProjectileSpawnPoint;
                speed = 7f;
                break;
        }

        if (selectedPrefab != null && spawnPoint != null)
        {
            GameObject instance = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D projRb = instance.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                projRb.velocity = dir * speed;
            }
            Destroy(instance, projectileDestroyTime);
        }

        yield return new WaitForSeconds(1.2f);
        Debug.Log("Slime Projectile Attack Ended");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                float knockbackForce = pushValue * 2f; // Adjust the knockback force as needed
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}

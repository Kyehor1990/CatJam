using System.Collections;
using UnityEngine;

public class bossMovement : MonoBehaviour
{
    public enum AttackPattern
    {
        JumpOnPlayer,
        StretchArm,
        SlimeProjectile
    }

    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public float patternDelay = 2f;

    public GameObject stretchArmPrefab;
    public GameObject lowProjectilePrefab;
    public GameObject midProjectilePrefab;
    public GameObject highProjectilePrefab;

    public Transform lowProjectileSpawnPoint;
    public Transform midProjectileSpawnPoint;
    public Transform highProjectileSpawnPoint;

    private bool isAttacking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(PatternLoop());
    }

    void Update()
    {
        FlipTowardsPlayer();

        if (!isAttacking && Vector2.Distance(transform.position, player.position) > attackRange)
        {
            FollowPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
        rb.linearVelocity = direction * moveSpeed;
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
                AttackPattern nextPattern = ChooseRandomPattern();
                yield return ExecutePattern(nextPattern);
                isAttacking = false;
            }

            yield return new WaitForSeconds(patternDelay);
        }
    }

    AttackPattern ChooseRandomPattern()
    {
        int patternCount = System.Enum.GetValues(typeof(AttackPattern)).Length;
        int randomIndex = Random.Range(0, patternCount);
        return (AttackPattern)randomIndex;
    }

    IEnumerator ExecutePattern(AttackPattern pattern)
    {
        switch (pattern)
        {
            case AttackPattern.JumpOnPlayer:
                yield return StartCoroutine(JumpOnPlayer());
                break;

            case AttackPattern.StretchArm:
                yield return StartCoroutine(StretchArm());
                break;

            case AttackPattern.SlimeProjectile:
                yield return StartCoroutine(SlimeProjectileAttack());
                break;
        }
    }

    IEnumerator JumpOnPlayer()
    {
        Debug.Log("Jump Attack Started");

        Vector2 jumpDir = new Vector2(player.position.x - transform.position.x, 0).normalized;
        float jumpForce = 15f;
        float jumpDuration = 0.4f;

        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            rb.linearVelocity = jumpDir * jumpForce;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.6f);
        Debug.Log("Jump Attack Ended");
    }

    IEnumerator StretchArm()
    {
        Debug.Log("Stretch Arm Attack Started");
        yield return new WaitForSeconds(0.2f);

        if (stretchArmPrefab != null)
        {
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            Quaternion rotation = direction == 1f ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

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
        int randomIndex = Random.Range(0, 3); // 0: low, 1: mid, 2: high

        switch (randomIndex)
        {
            case 0:
                if (lowProjectilePrefab != null)
                {
                    GameObject low = Instantiate(lowProjectilePrefab, lowProjectileSpawnPoint.position, Quaternion.identity);
                    Rigidbody2D rbLow = low.GetComponent<Rigidbody2D>();
                    if (rbLow != null) rbLow.linearVelocity = dir * 5f;
                    Destroy(low, 4f);
                }
                break;

            case 1:
                if (midProjectilePrefab != null)
                {
                    GameObject mid = Instantiate(midProjectilePrefab, midProjectileSpawnPoint.position, Quaternion.identity);
                    Rigidbody2D rbMid = mid.GetComponent<Rigidbody2D>();
                    if (rbMid != null) rbMid.linearVelocity = dir * 6f;
                    Destroy(mid, 4f);
                }
                break;

            case 2:
                if (highProjectilePrefab != null)
                {
                    GameObject high = Instantiate(highProjectilePrefab, highProjectileSpawnPoint.position, Quaternion.identity);
                    Rigidbody2D rbHigh = high.GetComponent<Rigidbody2D>();
                    if (rbHigh != null) rbHigh.linearVelocity = dir * 7f;
                    Destroy(high, 4f);
                }
                break;
        }

        yield return new WaitForSeconds(1.2f);
        Debug.Log("Slime Projectile Attack Ended");
    }
}

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
        Vector2 direction = (player.position - transform.position).normalized;
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
                AttackPattern nextPattern = ChooseRandomPattern();
                yield return ExecutePattern(nextPattern);
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
        isAttacking = true;

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

        isAttacking = false;
    }

    IEnumerator JumpOnPlayer()
    {
        Debug.Log("Jump Attack Started");
      //  animator.SetTrigger("Jump");

        Vector2 jumpDir = (player.position - transform.position).normalized;
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
       // animator.SetTrigger("Stretch");
        yield return new WaitForSeconds(0.2f);

        if (stretchArmPrefab != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            GameObject arm = Instantiate(stretchArmPrefab, midProjectileSpawnPoint.position, Quaternion.identity);
            arm.GetComponent<Rigidbody2D>().linearVelocity = dir * 8f;
            Destroy(arm, 4f); // 4 saniye sonra yok olsun
        }

        yield return new WaitForSeconds(1f);
        Debug.Log("Stretch Arm Attack Ended");
    }

    IEnumerator SlimeProjectileAttack()
    {
        Debug.Log("Slime Projectile Attack Started");
       // animator.SetTrigger("SlimeShoot");
        yield return new WaitForSeconds(0.2f);

        Vector2 dir = (player.position - transform.position).normalized;

        if (lowProjectilePrefab != null)
        {
            GameObject low = Instantiate(lowProjectilePrefab, lowProjectileSpawnPoint.position, Quaternion.identity);
            low.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f;
            Destroy(low, 4f);
        }

        if (midProjectilePrefab != null)
        {
            GameObject mid = Instantiate(midProjectilePrefab, midProjectileSpawnPoint.position, Quaternion.identity);
            mid.GetComponent<Rigidbody2D>().linearVelocity = dir * 6f;
            Destroy(mid, 4f);
        }

        if (highProjectilePrefab != null)
        {
            GameObject high = Instantiate(highProjectilePrefab, highProjectileSpawnPoint.position, Quaternion.identity);
            high.GetComponent<Rigidbody2D>().linearVelocity = dir * 7f;
            Destroy(high, 4f);
        }

        yield return new WaitForSeconds(1.2f);
        Debug.Log("Slime Projectile Attack Ended");
    }
}

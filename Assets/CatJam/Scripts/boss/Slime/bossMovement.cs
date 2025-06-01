using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public enum AttackPattern
    {
        StretchArm,
        SlimeProjectile
    }

    [Header("Player & Movement")]
    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 10f;

    [Header("Cooldown")]
    public float attackCooldown = 3f;

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
    private bool isCooldown = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private float attackFailSafeTimer = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ExecutePattern(AttackPattern.StretchArm);
        }

        if (!isAttacking && !isCooldown && player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            isAttacking = true;
            AttackPattern pattern = ChooseRandomPattern();
            ExecutePattern(pattern);
        }

        FlipTowardsPlayer();
    }

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (isAttacking)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                if (Vector2.Distance(transform.position, player.position) > attackRange)
                {
                    FollowPlayer();
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }

    void FollowPlayer()
    {
        if (player == null) return;

        Vector2 direction = new Vector2(player.position.x - transform.position.x, 0).normalized;
        rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;
        if (player.position.x < transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }

    AttackPattern ChooseRandomPattern()
    {
        int count = System.Enum.GetValues(typeof(AttackPattern)).Length;
        return (AttackPattern)Random.Range(0, count);
    }

    void ExecutePattern(AttackPattern pattern)
    {
        switch (pattern)
        {
            case AttackPattern.StretchArm:
                animator.SetTrigger("StretchArm");
                break;
            case AttackPattern.SlimeProjectile:
                animator.SetTrigger("ProjectileAttack");
                break;
        }

        CancelInvoke(nameof(ForceAttackEnd));
        Invoke(nameof(ForceAttackEnd), attackFailSafeTimer);
    }

    void ForceAttackEnd()
    {
        if (isAttacking)
        {
            Debug.LogWarning("Attack stuck, forcing reset.");
            OnAttackEnd();
        }
    }

    public void DoStretchArm()
    {
        if (stretchArmPrefab == null || midProjectileSpawnPoint == null) return;

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        Quaternion rotation = direction > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        GameObject arm = Instantiate(stretchArmPrefab, midProjectileSpawnPoint.position, rotation);
        arm.transform.parent = transform;

        Invoke(nameof(DestroyStretchArm), 2f);
    }

    void DestroyStretchArm()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains(stretchArmPrefab.name))
            {
                Destroy(child.gameObject);
            }
        }
        OnAttackEnd();
    }

    public void DoProjectileAttack()
    {
        if (player == null) return;

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

        Invoke(nameof(OnAttackEnd), 0.5f);
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
        isCooldown = true;

        rb.velocity = Vector2.zero;

        animator.ResetTrigger("StretchArm");
        animator.ResetTrigger("ProjectileAttack");

        CancelInvoke(nameof(ForceAttackEnd));
        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    void ResetCooldown()
    {
        isCooldown = false;
    }
}

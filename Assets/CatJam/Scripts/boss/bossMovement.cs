using System.Collections;
using UnityEngine;


public class bossMovement : MonoBehaviour
{
    public enum AttackPattern
    {
        Dash,
        Shoot,
        Summon
    }

    public Transform player;
    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public float patternDelay = 2f;

    private bool isAttacking = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            rb.linearVelocity = Vector2.zero;
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
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
            case AttackPattern.Dash:
                yield return StartCoroutine(DashAttack());
                break;

            case AttackPattern.Shoot:
                yield return StartCoroutine(ShootAttack());
                break;

            case AttackPattern.Summon:
                yield return StartCoroutine(SummonMinions());
                break;
        }

        isAttacking = false;
    }

    IEnumerator DashAttack()
    {
        Debug.Log("Dash Attack Started");

        Vector2 dashDir = (player.position - transform.position).normalized;
        float dashSpeed = 10f;
        float dashTime = 0.3f;

        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(0.5f); // Bekleme süresi
        Debug.Log("Dash Attack Ended");
    }

    IEnumerator ShootAttack()
    {
        Debug.Log("Shoot Attack Started");

        // Burada mermi instantiate edebilirsin:
        // Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        // Ya da yönlü ateşleme ekleyebilirsin.

        yield return new WaitForSeconds(1f);
        Debug.Log("Shoot Attack Ended");
    }

    IEnumerator SummonMinions()
    {
        Debug.Log("Summon Minions Started");

        // Instantiate(minionPrefab, transform.position + offset, Quaternion.identity);

        yield return new WaitForSeconds(1.5f);
        Debug.Log("Summon Minions Ended");
    }
}

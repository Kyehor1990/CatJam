using UnityEngine;

public class CubeBoss : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public GameObject spikePrefab;
    public Transform spikeSpawnPoint;

    public Transform player;

    public float attackCooldown = 3f;
    private float cooldownTimer = 0f;

    private Animator animator;
    private bool isAttacking = false;
    private int attackType = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        cooldownTimer = attackCooldown;
    }

    void Update()
    {
        if (player != null)
        {
            float scaleX = transform.localScale.x;
            if (player.position.x > transform.position.x)
                scaleX = Mathf.Abs(scaleX) * -1;
            else
                scaleX = Mathf.Abs(scaleX);

            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }

        if (isAttacking) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            ChooseRandomAttack();
        }
    }

    void ChooseRandomAttack()
    {
        attackType = Random.Range(0, 3); // 0: Lazer, 1: Mermi, 2: Diken
        isAttacking = true;

        // Saldırı türüne göre animasyon tetikleyelim
        switch (attackType)
        {
            case 0:
                animator.SetTrigger("LaserAttack");
                break;
            case 1:
                animator.SetTrigger("ProjectileAttack");
                break;
            case 2:
                animator.SetTrigger("SpikeAttack");
                break;
        }
    }

    public void PerformAttack()
    {
        switch (attackType)
        {
            case 0: // Lazer
                Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
                break;

            case 1: // Mermi
                GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                Vector2 direction = new Vector2(player.position.x - projectileSpawnPoint.position.x, 0).normalized;
                proj.GetComponent<ProjectileAttack>().SetDirection(direction);
                break;

case 2:
    GameObject spike = Instantiate(spikePrefab, spikeSpawnPoint.position, Quaternion.identity);
    Vector3 scale = spike.transform.localScale;
    if (player.position.x > transform.position.x)
{
    scale.x *= -1;
}

spike.transform.localScale = scale;
    break;
        }

        EndAttack();
    }

    public void EndAttack() // Bu da animasyon sonunda event olabilir
    {
        isAttacking = false;
        cooldownTimer = attackCooldown;
    }
}

using UnityEngine;

public class CubeBoss : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;

    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    public GameObject spikePrefab;

    public Transform player;

    public float attackCooldown = 3f;
    private float cooldownTimer = 0f;

    private Animator animator;
    private bool isAttacking = false;
    private int attackType = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0)
        {
            attackType = Random.Range(0, 3); // 0: Lazer, 1: Mermi, 2: Diken
            animator.SetInteger("AttackType", attackType);
            animator.SetBool("IsAttacking", true); // Animasyon başlatılır
            isAttacking = true;
        }
    }

    // Animasyon olayında çağrılacak
    public void PerformAttack()
    {
        switch (attackType)
        {
            case 0:
                Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                break;
            case 2:
                Vector3 spikePosition = new Vector3(player.position.x, transform.position.y, 0);
                Instantiate(spikePrefab, spikePosition, Quaternion.identity);
                break;
        }

        animator.SetBool("IsAttacking", false);
        isAttacking = false;
        cooldownTimer = attackCooldown;
    }
}

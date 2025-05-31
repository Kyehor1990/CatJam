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
        attackType = 1;
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

        /*if (cooldownTimer <= 0)
        {
            attackType = Random.Range(0, 3); // 0: Lazer, 1: Mermi, 2: Diken
            animator.SetInteger("AttackType", attackType);
            animator.SetBool("IsAttacking", true); // Animasyon başlatılır
            isAttacking = true;
        }*/

        if (Input.GetKeyDown(KeyCode.K))
        {
            PerformAttack();
            Debug.Log("Attack performed: " + attackType);
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
                Debug.Log("Projectile Attack");
                GameObject proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                Vector2 direction = new Vector2(player.position.x - projectileSpawnPoint.position.x, 0).normalized;
                proj.GetComponent<ProjectileAttack>().SetDirection(direction);

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

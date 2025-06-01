using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 5f;

    [SerializeField] int damage = 10;
    [SerializeField] int stress = 5;

    private Vector2 moveDirection;

public void SetDirection(Vector2 direction)
{
    moveDirection = direction.normalized;

    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
    transform.rotation = Quaternion.Euler(0f, 0f, angle + 180f);
}

void Update()
{
    transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
}

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (player != null && !player.isDodging)
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    playerHealth.AttackStress(stress);
                }
                Destroy(gameObject);
        }
    }
}
}

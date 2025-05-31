using UnityEngine;

public class Spike : MonoBehaviour
{
    public float lifetime = 1.5f;
    public int damage = 1;

    private bool hasDealtDamage = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasDealtDamage && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            hasDealtDamage = true;
        }
    }
}

using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public int damageAmount = 1;
    public int stressAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                playerHealth.AttackStress(stressAmount);
            }
        }
    }
}

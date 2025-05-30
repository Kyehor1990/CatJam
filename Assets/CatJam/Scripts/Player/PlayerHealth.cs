using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void TakeDamage(int amount)
    {
        if (playerMovement != null && playerMovement.IsDodging())
        {
            Debug.Log("Dodge aktif! Hasar alınmadı.");
            return;
        }

        currentHealth -= amount;
        Debug.Log("Hasar alındı! Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Öldü!");
    }
}

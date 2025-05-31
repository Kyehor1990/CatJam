using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    [SerializeField] Slider healthSlider;

    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        /*if (playerMovement != null && playerMovement.IsDodging())
        {
            Debug.Log("Dodge aktif! Hasar alınmadı.");
            return;
        }

        currentHealth -= amount;
        Debug.Log("Hasar alındı! Kalan can: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }*/

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    void Die()
    {
        Debug.Log("Öldü!");
    }
}

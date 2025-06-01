using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [SerializeField] Slider healthSlider;

    private PlayerMovement playerMovement;
    private PlayerEmote playerEmote;

    void Awake()
    {
        playerEmote = GetComponent<PlayerEmote>();
    }


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

void Update()
{
    if (Input.GetKeyDown(KeyCode.H))
    {
        TakeDamage(10);
    }
}

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        GetComponent<PlayerEmote>()?.InterruptEmote();

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void AttackStress(int amount)
    {
        if ( playerEmote.stress >= amount)
        {
            playerEmote.stress -= amount;
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

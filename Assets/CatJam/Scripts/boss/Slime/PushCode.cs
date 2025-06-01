using UnityEngine;

public class PushCode : MonoBehaviour
{
    [SerializeField] private float pushValue = 5f;

    private Rigidbody2D rb;

    public PlayerHealth playerHealth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false;  // Dinamik olmalı
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {   
                playerHealth.TakeDamage(10); // Oyuncuya hasar ver
                // Boss'tan oyuncuya yön (oyuncuya itme)
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;

                float knockbackForce = pushValue * 2f;

                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}

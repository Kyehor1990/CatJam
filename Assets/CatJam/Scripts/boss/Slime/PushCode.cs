using UnityEngine;

public class PushCode : MonoBehaviour
{
   
  [SerializeField] private float pushValue = 5f;



    private void Start()
    {
        // Ensure the Rigidbody2D is set to Kinematic if you want to control the push behavior manually
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Set to true if you want to control movement manually
        }
    }


private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
                float knockbackForce = pushValue * 2f;
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}

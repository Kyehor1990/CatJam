using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hızı ayarlamak için

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D bileşenini al
    }

    void Update()
    {
        // Input al
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D veya Sol/Sağ ok tuşları
        movement.y = Input.GetAxisRaw("Vertical");   // W/S veya Yukarı/Aşağı ok tuşları
        movement = movement.normalized; // Çapraz hareketleri sabitlemek için
    }

    void FixedUpdate()
    {
        // Rigidbody ile hareket ettir
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

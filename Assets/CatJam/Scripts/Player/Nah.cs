using UnityEngine;

public class Nah : MonoBehaviour
{
    public Transform target;
public Transform player;


    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    
    Vector3 scale = transform.localScale;

        // Oyuncunun x yönüne göre scale.x değerini tersle
        if (player.localScale.x > 0)
        {
            scale.x = Mathf.Abs(scale.x);
            scale.y = Mathf.Abs(scale.y);
        }
        else
        {
            scale.x = -Mathf.Abs(scale.x);
            scale.y = -Mathf.Abs(scale.y);
        }

    transform.localScale = scale;
}

}

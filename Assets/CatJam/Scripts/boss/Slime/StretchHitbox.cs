using System.Collections;
using UnityEngine;

public class StretchHitbox : MonoBehaviour
{
    public float stretchDuration = 0.5f;
    public float maxLength = 5f;
    public float stayDuration = 1.5f;

    private Vector3 initialScale;
    private Vector3 targetScale;

    void Start()
    {
        initialScale = transform.localScale;
        targetScale = new Vector3(maxLength, initialScale.y, initialScale.z);

        StartCoroutine(StretchAndDestroy());
    }

    IEnumerator StretchAndDestroy()
    {
        float elapsed = 0f;

        while (elapsed < stretchDuration)
        {
            float t = elapsed / stretchDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        yield return new WaitForSeconds(stayDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by arm!");
            // Buraya hasar verme kodunu ekleyebilirsin.
        }
    }
}

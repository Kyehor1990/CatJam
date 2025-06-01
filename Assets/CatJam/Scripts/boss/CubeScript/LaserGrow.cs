using UnityEngine;
using DG.Tweening;

public class LaserGrow : MonoBehaviour
{
    public float growDuration = 0.5f;
    public float maxScaleX = 8f;
    private bool isFlipped = false;

    public void StartGrow(bool flipDirection)
    {
        isFlipped = flipDirection;

        // İlk küçük başlasın
        transform.localScale = new Vector3(0.1f, transform.localScale.y, transform.localScale.z);

        float targetScaleX = maxScaleX;
        if (isFlipped)
            targetScaleX *= -1f;

        transform.DOScaleX(targetScaleX, growDuration).SetEase(Ease.OutSine)
                 .OnComplete(() => Destroy(gameObject, 0.5f));
    }
}

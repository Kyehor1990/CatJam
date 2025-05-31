using UnityEngine;
using DG.Tweening;

public class SpikeRise : MonoBehaviour
{
    public float riseDistance = 1f;
    public float riseDuration = 0.4f;
    public float stayDuration = 1f;
    public float fallDuration = 0.3f;

    void Start()
    {
        Vector3 startPos = transform.position;
        Vector3 hiddenPos = startPos - new Vector3(0, riseDistance, 0);

        transform.position = hiddenPos;

        transform.DOMoveY(startPos.y, riseDuration)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(stayDuration, () =>
                {
                    transform.DOMoveY(hiddenPos.y, fallDuration)
                        .SetEase(Ease.InBack)
                        .OnComplete(() =>
                        {
                            Destroy(gameObject);
                        });
                });
            });
    }
}

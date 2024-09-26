using DG.Tweening;
using UnityEngine;

public class NotifyImageComponent : MonoBehaviour
{
    [SerializeField] private GameObject notifyImageObject;
    [SerializeField] private float animationDuration = 0.2f;

    public void SetNotifySensorImage(float scaleValue)
    {
        notifyImageObject.transform.DOScale(scaleValue, animationDuration);
    }
}

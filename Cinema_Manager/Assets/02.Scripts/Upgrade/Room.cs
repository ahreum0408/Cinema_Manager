using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Room : MonoBehaviour, IOpenTarget {
    [SerializeField] private GameObject _openTarget;
    [SerializeField] private TargetType _targetType;

    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

    public void ActiveObj(bool active, bool on = false) {
        _isOpen = active;
        gameObject.SetActive(!active);
        _openTarget.gameObject.SetActive(active);
        //LevelEvents.ChangeRoomActiveEvent?.Invoke(this, active);
        ScaleSetting(active);
    }

    public void ScaleSetting(bool active)
    {
        float time = 0.5f;
        Vector3 originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, time).SetEase(Ease.OutBack).OnComplete(()=> LevelEvents.ChangeRoomActiveEvent?.Invoke(this, active));
    }
}

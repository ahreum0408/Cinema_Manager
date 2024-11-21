using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SignBoard : MonoBehaviour, IOpenTarget
{
    [SerializeField] private PlayableDirector _startTimeline;
    [SerializeField] private BoxCollider _collider;

    private MoneyDummy _moneyDummy;

    [SerializeField] private TargetType _targetType;
    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

    private void Awake()
    {
        _moneyDummy = GetComponentInChildren<MoneyDummy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetInitMoney();
        }
    }

    private void SetInitMoney()
    {
        _moneyDummy.AddMoneyObject(36);
    }

    public void ActiveObj(bool active, bool on = false) {
        // Timeline
        if (true == active && false == gameObject.activeSelf)
        {
            _startTimeline.Play();
            _collider.enabled = false;
        }

        _isOpen = active;
        gameObject.SetActive(active);
        if (gameObject.activeSelf == true)
            _collider.enabled = false;
        else
            _collider.enabled = true;
        LevelEvents.ChangeSignBoardActiveEvent?.Invoke(this, active);
        ScaleSetting();
    }

    public void ScaleSetting() {
        float time = 0.5f;
        Vector3 originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, time).SetEase(Ease.OutBack);
    }
}

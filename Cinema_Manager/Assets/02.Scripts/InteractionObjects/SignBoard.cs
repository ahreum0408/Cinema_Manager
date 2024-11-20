using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBoard : MonoBehaviour, IOpenTarget
{
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
        _isOpen = active;
        gameObject.SetActive(active);
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

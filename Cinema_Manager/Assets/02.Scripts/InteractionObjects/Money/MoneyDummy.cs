using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AyunDefine;

public class MoneyDummy : MonoBehaviour, IIneractionable
{
    private Stack<Money> _moneyStack;

    private int _moneyAmount => _moneyStack.Count;

    // Bool
    private bool _isClearing = false;

    public Transform _playerTrm;
    private PlayerController _playerController;

    private readonly Vector3 _moneyRotation = new Vector3(0, 90, 0);

    [Header("Spacing")]
    [SerializeField] private float _spacingX = 0.85f;
    [SerializeField] private float _spacingY = 0.25f;
    [SerializeField] private float _spacingZ = 0.5f; // 사용할 땐 음수로

    private void Awake()
    {
        _moneyStack = new Stack<Money>();

        _playerController = PlayerManager.Instance.PlayerController;
        _playerTrm = PlayerManager.Instance.Transform;
    }

    public void EnterInteraction(AgentController agent)
    {
        if (_moneyAmount > 0 && false == _isClearing)
        {
            _playerController.OnGetPaid?.Invoke(_moneyAmount);
            StartCoroutine(ClearMoneyObject());
        }
    }

    public void ExitInteraction(AgentController agent) { }

    public IEnumerator ClearMoneyObject()
    {
        _isClearing = true;

        foreach (Money money in _moneyStack)
        {
            money.JumpToPositionAndDestory(_playerTrm.localPosition);
            yield return new WaitForSeconds(0.02f);
        }

        _moneyStack.Clear();
        _isClearing = false;
    }

    // 지불 (돈 생성)
    public void AddMoneyObject(int newMoneyAmount)
    {
        // isClearing 될 때 까지 기다려야함
        StartCoroutine(AddMoneyRoutine(newMoneyAmount));
    }

    private IEnumerator AddMoneyRoutine(int newMoneyAmount)
    {
        while (_isClearing)
        {
            yield return null;
        }

        for (int i = 0; i < newMoneyAmount; i++)
        {
            GameObject money = PoolManager.Instance.Pop(PoolableType.Money.ToString(), transform,
                GetMoneyPosition(), Quaternion.Euler(_moneyRotation));
            _moneyStack.Push(money.transform.GetComponent<Money>());
        }
    }

    public Vector3 GetMoneyPosition()
    {
        Vector3 moneyPosition = Vector3.zero;

        // 3x3
        int row = (_moneyAmount % 9) / 3;
        int columun = (_moneyAmount % 9) % 3;
        int height = _moneyAmount / 9;

        moneyPosition.x = _spacingX * columun;
        moneyPosition.y = _spacingY * height;
        moneyPosition.z = -_spacingZ * row;
        return moneyPosition;
    }
}
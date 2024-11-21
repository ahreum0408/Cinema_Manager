using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class MoneyDummy : MonoBehaviour, IIneractionable
{
    private Stack<Money> _moneyStack;
    [SerializeField] private int _price = 2; // 한개당 가격이 얼마인지

    [HideInInspector] public GameObject GameObject => gameObject;
    public bool IsAddMoney => _isAddMoney;
    private int _moneyAmount => _moneyStack.Count;

    // Bool
    private bool _isClearing = false;
    private bool _isAddMoney = false;

    private readonly Vector3 _moneyRotation = new Vector3(0, 90, 0);

    private Transform _moneySpawnTrm;

    [Header("Spacing")]
    [SerializeField] private float _spacingX = 0.85f;
    [SerializeField] private float _spacingY = 0.25f;
    [SerializeField] private float _spacingZ = 0.5f; // 사용할 땐 음수로

    private void Awake()
    {
        _moneySpawnTrm = transform.Find("MoneySpawnTrm").GetComponent<Transform>();
        _moneyStack = new Stack<Money>();
    }

    public void EnterInteraction(AgentController agent)
    {
        if (_moneyAmount > 0 && false == _isClearing)
        {
            StartCoroutine(ClearMoneyObject(agent));
        }
    }

    public void ExitInteraction(AgentController agent) { }

    public IEnumerator ClearMoneyObject(AgentController agent)
    {
        if (agent.TryGetComponent(out PlayerController player))
        {
            _isClearing = true;

            player.OnGetPaid?.Invoke(_moneyAmount * _price);

            foreach (Money money in _moneyStack)
            {
                money.JumpToPositionAndDestory(player.transform.position);
                Vibration.Vibrate(10, 32);
                yield return new WaitForSeconds(0.01f);
            }

            _moneyStack.Clear();
            _isClearing = false;
        }
    }

    // 지불 (돈 생성)
    public void AddMoneyObject(int newMoneyAmount)
    {
        if (IsAddMoney) return;

        _isAddMoney = true;
        StartCoroutine(AddMoneyRoutine(newMoneyAmount));
    }

    private IEnumerator AddMoneyRoutine(int newMoneyAmount)
    {
        for (int i = 0; i < newMoneyAmount; i++)
        {
            GameObject money = PoolManager.Instance.Pop(PoolableType.Money.ToString(), _moneySpawnTrm,
                Vector3.zero, Quaternion.Euler(_moneyRotation));

            if (money.TryGetComponent(out ITakeable takeable))
                takeable.Take(transform, GetMoneyPosition(), _moneyRotation, 1f);

            // isClearing 될 때 까지 기다려야함
            yield return new WaitUntil(() => !_isClearing);
            _moneyStack.Push(money.transform.GetComponent<Money>());

            yield return new WaitForSeconds(0.15f);
        }
        _isAddMoney = false;
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
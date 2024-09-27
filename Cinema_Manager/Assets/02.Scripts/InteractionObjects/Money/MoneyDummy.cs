using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class MoneyDummy : MonoBehaviour, IIneractionable
{
    private Stack<Money> moneyStack;

    private int moneyAmount => moneyStack.Count;

    // Bool
    private bool isClearing = false;

    private Transform playerTransform;
    private PlayerController playerController;

    private readonly Vector3 moneyRotation = new Vector3(0, 90, 0);

    [Header("Spacing")]
    [SerializeField] private float spacingX = 0.85f;
    [SerializeField] private float spacingY = 0.25f;
    [SerializeField] private float spacingZ = 0.5f; // 사용할 땐 음수로

    private void Awake()
    {
        moneyStack = new Stack<Money>();
    }

    private void Start()
    {
        playerTransform = transform.Find("Player").GetComponent<Transform>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void EnterInteraction()
    {
        if (moneyAmount > 0 && false == isClearing)
        {
            playerController.OnGetPaid?.Invoke(moneyAmount);
            ClearMoneyObject();
        }
    }

    public void ExitInteraction() { }

    public void AddMoneyObject(int newMoneyAmount)
    {
        // isClearing 될 때 까지 기다려야함
        if (isClearing == false) return;

        for (int i = 0; i < newMoneyAmount; i++)
        {
            GameObject money = PoolManager.Instance.Pop(PoolableType.Money.ToString(),
                GetMoneyPosition(), Quaternion.Euler(moneyRotation));
            money.transform.SetParent(transform);
            moneyStack.Push(money.transform.GetComponent<Money>());
        }
    }

    public Vector3 GetMoneyPosition()
    {
        Vector3 moneyPosition = Vector3.zero;

        int row = (moneyAmount % 9) / 3;
        int columun = (moneyAmount % 9) % 3;
        int height = moneyAmount / 9;

        moneyPosition.x = spacingX * columun;
        moneyPosition.y = spacingY * height;
        moneyPosition.z = -spacingZ * row;

        return moneyPosition;
    }

    public IEnumerator ClearMoneyObject()
    {
        isClearing = true;

        foreach (Money money in moneyStack)
        {
            money.JumpToPositionAndDestory(playerTransform.localPosition);
            yield return new WaitForSeconds(0.02f);
        }

        moneyStack.Clear();
        isClearing = false;
    }
}
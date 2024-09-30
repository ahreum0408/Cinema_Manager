using BehaviorDesigner.Runtime;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

using static AyunDefine;

public enum CustomerType
{
    Basic = 0, Call, Sleep, Thief
}

public class CustomerData
{
    [Header("Customer Type")]
    public bool isBuy = false; // 구매 완료? 물건 다 받았냐
    public bool isSeat; // 식탁을 사용하는 손님인가?
    public bool isBad; // 진상 손님인가?


    [Header("Buy Type")]
    public PoolableType objectType;
    public int maxBuySum = 3;
    public int wantBuy; // 원하는 수량
}

public class Customer : AgentController
{
    public CustomerData customerData;
    public int currentBuy; // 현재 가진 수량

    [HideInInspector] public bool IsStacked => StackCompo.IsStacked;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Chair currentChair;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public Counter Counter {  get; private set; }
    public Table Table { get; private set; }
    public CustomerType CurrentCustomerType { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    // Events
    public Action<ITakeable> OnTakeFood;
    public Func<ITakeable> OnGiveFood;

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        StackCompo = GetComponent<AgentStackComponent>();
        AnimationCompo = GetComponent<AgentAnimationComponent>();

        Counter = FindObjectOfType<Counter>();
        Table = FindObjectOfType<Table>();
    }

    private void Start()
    {
        startPos = transform.position;
        SetSeat();
        SetCustomerType();
        SelectObjectType();
        SelectBuySum();

        OnTakeFood += HandleTakeFood;
        OnGiveFood += HandleGiveFood;
    }

    #region Set Customer Type
    // 먹고 가는 손님
    private void SetSeat()
    {
        int rand = Random.Range(0, 2);
        if (rand > 0)
            isSeat = false;
        else
            isSeat = true;
    }
    
    // 손님 타입(진상 손님 종류)
    private void SetCustomerType()
    {
        int rand = Random.Range(0, 10);
        if (rand > 0)
            isBad = false;
        else
            isBad = true;

        if(isBad)
        {
            rand = Random.Range(1, 3);
            switch(rand)
            {
                case 1:
                    CurrentCustomerType = CustomerType.Call;
                    break;
                case 2:
                    CurrentCustomerType = CustomerType.Sleep;
                    break;
            }
        }
        else
            CurrentCustomerType = CustomerType.Basic;
    }

    // 손님 원하는 물건
    private void SelectObjectType()
    {
        objectType = PoolableType.TriangleKimbap;
    }

    // 구매 수량
    private void SelectBuySum()
    {
        wantBuy = Random.Range(1, maxBuySum + 1);
    }
    #endregion


    #region Handle

    private void HandleTakeFood(ITakeable takeable)
    {
        if (IsStacked == false)
            AnimationCompo.UpperHoldingAnimation(true);

        // UI Update
        //OnStackMaxed?.Invoke(IsStackMax);
    }

    private Food HandleGiveFood()
    {
        Food food = StackCompo.GetTopObject() as Food;

        // UI Update
        //OnStackMaxed?.Invoke(IsStackMax);

        if (IsStacked == false)
            AnimationCompo.UpperHoldingAnimation(false);

        return food;
    }

    #endregion
}

public class SharedCustomer : SharedVariable<Customer>
{
    public static implicit operator SharedCustomer(Customer value)
    {
        return new SharedCustomer { Value = value };
    }
}

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
    public bool isGet = false; // 물건을 다 받았는가?
    public bool isBuy = false; // 구매 완료? 물건 다 받았냐
    public bool isSeat; // 식탁을 사용하는 손님인가?
    public bool isBad; // 진상 손님인가?


    [Header("Buy Type")]
    public PoolableType objectType;
    public int maxBuySum = 3;
}

public class Customer : AgentController
{
    public CustomerData customerData;

    [HideInInspector] public bool IsStacked => StackCompo.IsStacked;
    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public DisplayStand currentStand;
    [HideInInspector] public Point currentChair;

    // Components
    public NavMeshAgent Agent { get; private set; }
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
    }

    private void Start()
    {
        customerData = new CustomerData();

        startPos = transform.position;
        SetSeat();
        SetCustomerType();
        SelectBuySum();
        SelectObjectType();

        OnTakeFood += HandleTakeFood;
        OnGiveFood += HandleGiveFood;
    }

    #region Set Customer Type
    // 먹고 가는 손님
    private void SetSeat()
    {
        int rand = Random.Range(0, 2);
        if (rand > 0)
            customerData.isSeat = false;
        else
            customerData.isSeat = true;
    }
    
    // 손님 타입(진상 손님 종류)
    private void SetCustomerType()
    {
        int rand = Random.Range(0, 10);
        if (rand > 0)
            customerData.isBad = false;
        else
            customerData.isBad = true;

        if(customerData.isBad)
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
    public void SelectObjectType()
    {
        int rand = Random.Range(1, 5);
        customerData.objectType = (PoolableType)rand;
        if(ObjectManager.Instance.FindDisplayStand(customerData.objectType).CanStandPoint() == null)
            SelectObjectType();
    }

    // 구매 수량
    private void SelectBuySum()
    {
        StackCompo.SetMaxStackCount(Random.Range(1, customerData.maxBuySum + 1));
    }
    #endregion


    #region Handle

    private void HandleTakeFood(ITakeable takeable)
    {
        if (IsStacked == false)
            AnimationCompo.UpperHoldingAnimation(true);
    }

    private Food HandleGiveFood()
    {
        Food food = StackCompo.GetTopObject() as Food;

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

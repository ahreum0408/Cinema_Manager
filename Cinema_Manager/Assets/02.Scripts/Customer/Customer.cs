using BehaviorDesigner.Runtime;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum CustomerType
{
    Basic = 0, Call, Sleep, Thief
}

public class Customer : AgentController
{
    [Header("Customer Type")]
    [SerializeField] private Material[] mat = new Material[2];
    public bool isBuy = false; // 구매 완료? 물건 다 받았냐
    public bool isSeat; // 식탁을 사용하는 손님인가?
    public bool isBad; // 진상 손님인가?
    public bool IsStacked => StackCompo.IsStacked;

    [Header("Buy Type")]
    // 나중에 물건 타입도 추가?
    [SerializeField] private int maxBuySum = 3;
    public int wantBuy; // 원하는 수량
    public int currentBuy; // 현재 받은 수량

    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Chair currentChair;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public SeoyeonCounter Counter {  get; private set; }
    public Table Table { get; private set; }
    public CustomerType CurrentCustomerType { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    // Events
    public Action<ITakeable> OnTakeFood;
    public Func<ITakeable> OnGiveFood;

    private MeshRenderer _meshRenderer;
    int i = 0;

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        StackCompo = GetComponent<AgentStackComponent>();
        AnimationCompo = GetComponent<AgentAnimationComponent>();

        Counter = FindObjectOfType<SeoyeonCounter>();
        Table = FindObjectOfType<Table>();

        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        startPos = transform.position;
        SetSeat();
        SetCustomerType();
        SelectBuySum();

        OnTakeFood += HandleTakeFood;
        OnGiveFood += HandleGiveFood;
    }

    private void SetSeat()
    {
        int rand = Random.Range(0, 2);
        if (rand > 0)
            isSeat = false;
        else
            isSeat = true;
    }

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

    private void SelectBuySum()
    {
        wantBuy = Random.Range(1, maxBuySum + 1);
    }

    public void ChangeCustomerMat()
    {
        i = ++i % 2;

        _meshRenderer.material = mat[i];
    }

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

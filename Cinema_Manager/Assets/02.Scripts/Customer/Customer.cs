using BehaviorDesigner.Runtime;
using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

using static AyunDefine;
using UnityEngine.UI;

public enum CustomerType
{
    Basic = 0, Parcel, Call, Sleep, Thief
}

public class CustomerData
{
    [Header("Customer Type")]
    public bool isGive = false; // 물건을 선택 할 수 있나
    public bool isBuy = false; // 계산 가능한 상태인가?
    public bool isCalculate = false; // 계산을 해줬는가?
    public bool isSeat; // 식탁을 사용하는 손님인가?
    public bool isBad; // 진상 손님인가?


    [Header("Buy Type")]
    public PoolableType objectType;
    public int maxBuySum = 3;
}

public class Customer : AgentController
{
    public CustomerData customerData;

    public CustomerType CurrentCustomerType;

    public float defualtSpeed = 3.5f;

    public float SpacingY = 0f;
    public bool IsFood = false;
    private int foodTypeSum = 1;


    [SerializeField] private LayerMask _whatIsPlayer;

    [SerializeField] private Canvas badCustomerCanvas;
    [SerializeField] private Image gauge;

    [HideInInspector] public bool IsStacked => StackCompo.IsStacked;
    [HideInInspector] public Vector3 startPos;

    [HideInInspector] public DisplayStand currentStand;
    [HideInInspector] public Point currentChair;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        StackCompo = GetComponent<AgentStackComponent>();
        AnimationCompo = GetComponent<AgentAnimationComponent>();
    }

    protected override void OnEnable()
    {
        customerData = new CustomerData();
    }

    private void Start()
    {
        Agent.speed = defualtSpeed;

        startPos = CustomerSpawnManager.Instance.transform.position;

        SetSeat();
        SelectBuySum();
        SelectObjectType();
        SetCustomerType();

        OnTakeTakeable += HandleTakeTakeable;
        OnGiveTakeable += HandleGiveTakeable;
    }

    private void Update()
    {
        SetMoveAniamtion();
    }
    private void SetMoveAniamtion()
    {
        if (Agent.velocity.sqrMagnitude > 0)
            AnimationCompo.SetMovementAnimation(Agent.destination);
        else
            AnimationCompo.SetMovementAnimation(Vector3.zero);
    }

    public bool CheckPlayer()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 4f, _whatIsPlayer);
        if (col.Length > 0)
            return true;
        else
            return false;
    }

    public bool CanSetDestination()
    {
        return !Agent.isPathStale && 
                Vector3.Distance(Agent.destination, transform.position) < Agent.stoppingDistance + 0.1f && 
                Agent.velocity.sqrMagnitude < 0.01f;
    }

    public void SetFoodTypeSum()
    {
        foodTypeSum++;
    }

    #region SetUI
    public void SetCanvas(bool isCanvas)
    {
        badCustomerCanvas.enabled = isCanvas;
    }

    public void SetGauge(float value)
    {
        gauge.fillAmount = value;
    }

    #endregion

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
    
    // 손님 타입
    private void SetCustomerType()
    {
        if (CurrentCustomerType == CustomerType.Basic || CurrentCustomerType == CustomerType.Parcel)
            return;

        if(CurrentCustomerType == CustomerType.Sleep)
            customerData.isSeat = true;

        customerData.isBad = true;
    }

    // 손님 원하는 물건
    public void SelectObjectType()
    {
        int maxAttempts = 10;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            int rand = Random.Range(1, foodTypeSum + 1);
            customerData.objectType = (PoolableType)rand;

            DisplayStand stand = ObjectManager.Instance.FindDisplayStand(customerData.objectType);

            if (stand != null && stand.CanStandPoint() != null)
            {
                currentStand = stand;
                break;
            }

            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            foreach (var objType in Enum.GetValues(typeof(PoolableType)))
            {
                DisplayStand stand = ObjectManager.Instance.FindDisplayStand((PoolableType)objType);
                if (stand != null && stand.CanStandPoint() != null)
                {
                    customerData.objectType = (PoolableType)objType;
                    currentStand = stand;
                    break;
                }
            }
        }
    }

    // 구매 수량
    private void SelectBuySum()
    {
        StackCompo.SetMaxStackCount(Random.Range(1, customerData.maxBuySum + 1));
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

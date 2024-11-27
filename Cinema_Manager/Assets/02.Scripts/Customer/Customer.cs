using BehaviorDesigner.Runtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static AyunDefine;
using BehaviorTree = BehaviorDesigner.Runtime.BehaviorTree;
using Random = UnityEngine.Random;

public enum CustomerType
{
    Basic = 0, Parcel, Call, Sleep, Thief
}

public class CustomerData
{
    [Header("Customer Type")]
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
    private int _foodTypeSum = 1;


    [SerializeField] private LayerMask _whatIsPlayer;

    [SerializeField] private Canvas badCustomerCanvas;
    [SerializeField] private Image gauge;

    [SerializeField] private Canvas foodCanvas;
    [SerializeField] private TextMeshProUGUI foodNumText;
    [SerializeField] private Image foodImage;
    [SerializeField] private List<Sprite> foodSprites;

    [HideInInspector] public bool IsStacked => StackCompo.IsStacked;
    [HideInInspector] public Vector3 startPos;

    [HideInInspector] public DisplayStand currentStand;
    [HideInInspector] public Point currentChair;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    private BehaviorTree behaviorTree;

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        StackCompo = GetComponent<AgentStackComponent>();
        AnimationCompo = GetComponent<AgentAnimationComponent>();
        behaviorTree = GetComponent<BehaviorTree>();
    }

    protected override void OnEnable()
    {
        _foodTypeSum = CustomerSpawnManager.Instance.FoodTypeSum;

        customerData = new CustomerData();
        RestartBehaviorTree();
        Agent.speed = defualtSpeed;

        startPos = CustomerSpawnManager.Instance.transform.position;

        SetSeat();
        SelectBuySum();
        SelectObjectType();
        SetCustomerType();

        OnTakeTakeable += HandleTakeTakeable;
        OnGiveTakeable += HandleGiveTakeable;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnTakeTakeable -= HandleTakeTakeable;
        OnGiveTakeable -= HandleGiveTakeable;
    }

    private void Update()
    {
        SetMoveAniamtion();
    }

    public void RestartBehaviorTree()
    {
        behaviorTree.DisableBehavior();
        behaviorTree.EnableBehavior();
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

        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].GetComponent<PlayerController>() != null)
                return true;
        }
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
        _foodTypeSum++;
    }

    #region SetUI
    public void SetBadCanvas(bool isCanvas)
    {
        badCustomerCanvas.enabled = isCanvas;
    }

    public void SetGauge(float value)
    {
        gauge.fillAmount = value;
    }

    public void SetFoodCanvas(bool isCanvas)
    {
        foodCanvas.enabled = isCanvas;
    }

    public void SetFoodNumText(string text)
    {
        foodNumText.text = text;
    }

    private void SetFoodImage()
    {
        foodImage.sprite = foodSprites[(int)customerData.objectType - 1];
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

        if (CurrentCustomerType == CustomerType.Sleep)
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
            int rand = Random.Range(1, _foodTypeSum + 1);
            customerData.objectType = (PoolableType)rand;

            DisplayStand stand = ObjectManager.Instance.FindDisplayStand(customerData.objectType);

            if (stand != null)
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
                if (stand != null)
                {
                    customerData.objectType = (PoolableType)objType;
                    currentStand = stand;
                    break;
                }
            }
        }
        if (CurrentCustomerType != CustomerType.Parcel)
            SetFoodImage();
    }

    // 구매 수량
    private void SelectBuySum()
    {
        StackCompo.SetMaxStackCount(Random.Range(1, customerData.maxBuySum + 1));
        if (CurrentCustomerType != CustomerType.Parcel)
            SetFoodNumText(StackCompo.MaxStackCount.ToString());
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

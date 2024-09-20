using System;
using UnityEngine;
using UnityEngine.Events;
using static AyunDefine;

public class PlayerData
{
    private int moneyAmount = 100;
    public int MoneyAmount
    {
        get => moneyAmount;
        set => moneyAmount = value;
    }
}

public class PlayerController : AgentController
{
    [SerializeField] private FloatingJoystick _joystick;
    private PlayerData _playerData;

    private readonly int _maxMoneyAmount = 9999;
    public int MoneyAmount => _playerData.MoneyAmount;

    private AgentMovementComponent _agentMovement;
    private AgentAnimationComponent _agentAnimation;
    //private AgentStackComponent stackComponent;

    // Property
    //public bool IsStackMax => stackComponent.IsStackMax;
    //public bool IsStacked => stackComponent.IsStacked;

    // Events
    //public Action<Bread> OnTakeBread;
    //public Func<Bread> OnGiveBread;

    public Action<int> OnGetPaid;
    public Func<int, int> OnPaidCost;

    // UnityEvents
    public UnityEvent<int> OnMoneyAmountValueChanged;
    public UnityEvent<bool> OnStackMaxed;

    private bool isPlay = false;

    #region Main
    protected override void Init()
    {
        _playerData = new PlayerData();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = transform.Find("Visual").GetComponent<Animator>();
    }

    protected override void SetAgentComponents()
    {
        base.SetAgentComponents();

        _agentMovement = GetAgentComponent<AgentMovementComponent>();
        _agentAnimation = GetAgentComponent<AgentAnimationComponent>();
        //stackComponent = GetAgentComponent<AgentStackComponent>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        //OnTakeBread += HandleTakeBread;
        //OnGiveBread += HandleGiveBread;
        //OnGetPaid += HandleOnGetPaid;
        //OnPaidCost += HandleOnPaidCost;
    }

    private void Start()
    {
        OnMoneyAmountValueChanged?.Invoke(_playerData.MoneyAmount);
    }

    private void Update()
    {
        HandleInputVaueChanged();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //OnTakeBread -= HandleTakeBread;
        //OnGiveBread -= HandleGiveBread;
        //OnGetPaid -= HandleOnGetPaid;
        //OnPaidCost -= HandleOnPaidCost;
    }
    #endregion


    #region Handle
    private void HandleInputVaueChanged()
    {
        float xInput = _joystick.Horizontal;
        float zInput = _joystick.Vertical;
        Vector3 inputValue = new Vector3(xInput, 0, zInput).normalized;

        _agentMovement.SetVelocity(inputValue);
        _agentAnimation.SetMovementAnimation(inputValue);
    }

    /*
    private void HandleTakeBread(Bread bread)
    {
        stackComponent.TakeObject(bread);
        _agentAnimation.SetStackState();

        // UI Update
        OnStackMaxed?.Invoke(IsStackMax);
    }

    private Bread HandleGiveBread()
    {
        Bread bread = stackComponent.GetTopObject() as Bread;

        // UI Update
        OnStackMaxed?.Invoke(IsStackMax);

        this.FrameDelayAfterAction(5, () =>
        {
            if (false == stackComponent.IsStacked)
            {
                _agentAnimation.SetDefaultState();
            }
        }).Forget();

        return bread;
    }

    private void HandleOnGetPaid(int moneyAmount)
    {
        int newMoneyAmount = Mathf.Clamp(_playerData.MoneyAmount + moneyAmount, 0, _maxMoneyAmount);
        _playerData.MoneyAmount = newMoneyAmount;

        // UI Update
        OnMoneyAmountValueChanged?.Invoke(_playerData.MoneyAmount);
    }

    private int HandleOnPaidCost(int cost)
    {
        int beforeMoneyAmount = _playerData.MoneyAmount;
        _playerData.MoneyAmount = _playerData.MoneyAmount - cost;

        // UI Update
        OnMoneyAmountValueChanged?.Invoke(_playerData.MoneyAmount);

        return beforeMoneyAmount;
    }
    */
    #endregion
}

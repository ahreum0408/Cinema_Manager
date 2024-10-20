using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static AyunDefine;

public class PlayerController : AgentController
{
    [SerializeField] private FloatingJoystick _joystick;
    [SerializeField] private TextMeshProUGUI _stackMaxText;

    private readonly int _maxMoneyAmount = 9999;

    private AgentMovementComponent _agentMovement;
    private AgentAnimationComponent _agentAnimation;
    private AgentStackComponent _stackComponent;

    // Property
    public bool IsStackMax => _stackComponent.IsStackMax;
    public bool IsStacked => _stackComponent.IsStacked;

    // Events
    public Action<ITakeable, PoolableType, float, bool> OnTakeTakeable;
    public Func<ITakeable> OnGiveTakeable;

    public Action<int> OnGetPaid;
    public Func<int, int> OnPaidCost;

    public Action<bool> OnStackMaxed;

    // UnityEvents
    //public UnityEvent<bool> OnStackMaxed;

    private bool isPlay = false;

    #region Main
    protected override void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = transform.Find("Visual").GetComponent<Animator>();
    }

    protected override void SetAgentComponents()
    {
        base.SetAgentComponents();

        _agentMovement = GetAgentComponent<AgentMovementComponent>();
        _agentAnimation = GetAgentComponent<AgentAnimationComponent>();
        _stackComponent = GetAgentComponent<AgentStackComponent>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        OnTakeTakeable += HandleTakeTakeable;
        OnGiveTakeable += HandleGiveTakeable;
        OnGetPaid += HandleOnGetPaid;
        OnPaidCost += HandleOnPaidCost;
        OnStackMaxed += HandleStackMaxed;
    }

    private void Update()
    {
        HandleInputVaueChanged();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        OnTakeTakeable -= HandleTakeTakeable;
        OnGiveTakeable -= HandleGiveTakeable;
        OnGetPaid -= HandleOnGetPaid;
        OnPaidCost -= HandleOnPaidCost;
        OnStackMaxed -= HandleStackMaxed;
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

    public bool CanTakeFood(PoolableType type)
    {
        bool isSameType = _stackComponent.CurrentHoldType == PoolableType.None
            || _stackComponent.CurrentHoldType == type;

        return isSameType && !IsStackMax;
    }

    private void HandleTakeTakeable(ITakeable takeable, PoolableType type, float spacingY, bool isDrink)
    {
        if (IsStacked == false)
            _agentAnimation.UpperHoldingAnimation(true);

        _stackComponent.TakeObject(takeable, type, spacingY, isDrink);

        // UI Update
        OnStackMaxed?.Invoke(IsStackMax);
    }

    public bool CanGiveFood(PoolableType type)
    {
        return _stackComponent.CurrentHoldType == type && IsStacked;
    }

    private ITakeable HandleGiveTakeable()
    {
        ITakeable food = _stackComponent.GetTopObject();

        // UI Update
        OnStackMaxed?.Invoke(IsStackMax);

        if (IsStacked == false)
            _agentAnimation.UpperHoldingAnimation(false);

        return food;
    }

    private void HandleStackMaxed(bool isStackMax)
    {
        _stackMaxText.enabled = isStackMax;
        if (isStackMax)
        {
            // 위치 제발....
            Debug.Log(_stackComponent.TopObjPos);
            Camera mainCam = Camera.main;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(mainCam, _stackComponent.TopObjPos);
            _stackMaxText.transform.localPosition = screenPos;
            //_stackMaxText.rectTransform.localPosition = new Vector2(screenPos.x / 2, screenPos.y) / 2;
            //_stackMaxText.rectTransform.localPosition = new Vector2(_stackMaxText.rectTransform.anchoredPosition.x, screenPos.y);
        }
    }


    private void HandleOnGetPaid(int moneyAmount)
    {
        // 돈 받았을 때 이벤트 처리 해주기
        // UI Update
    }

    private int HandleOnPaidCost(int cost)
    {
        // 돈 냈을 때 이벤트 처리 해주기
        // UI Update

        return 0;
    }
    #endregion
}

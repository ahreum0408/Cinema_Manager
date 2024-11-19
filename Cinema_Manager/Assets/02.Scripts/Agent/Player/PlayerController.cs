using System;
using System.Collections;
using TMPro;
using UnityEngine;
using static AyunDefine;
using static LevelEvents;

public class PlayerController : AgentController
{
    [Header("INPUT")]
    [SerializeField] private FloatingJoystick _joystick;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _stackMaxText;

    // Component
    private AgentMovementComponent _agentMovement;
    private AgentAnimationComponent _agentAnimation;
    private AgentStackComponent _stackComponent;

    // Property
    public bool IsStackMax => _stackComponent.IsStackMax;
    public bool IsStacked => _stackComponent.IsStacked;

    public Action<int> OnGetPaid;
    public Func<int, int> OnPaidCost;

    public Action<bool> OnStackMaxed;

    private float _sellingCostWeigth;

    // UnityEvents
    //public UnityEvent<bool> OnStackMaxed;

    #region Main
    protected override void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = transform.Find("Visual").GetComponent<Animator>();

        _stackMaxText.enabled = false;
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

        PriceChangingEvent += HandlePriceChangingEvent;
    }

    private IEnumerator PriceChangingRoutine(Transform moveTrm)
    {
        GameObject money = PoolManager.Instance.Pop(PoolableType.Money.ToString(), transform);
        if (money != null && money.TryGetComponent(out ITakeable takeable))
        {
            takeable.Take(null, moveTrm.position, Vector3.zero);
            yield return new WaitForSeconds(0.4f);
            PoolManager.Instance.Push(PoolableType.Money.ToString(), money);
        }
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

        PriceChangingEvent -= HandlePriceChangingEvent;
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

    protected override void HandleTakeTakeable(ITakeable takeable, PoolableType type, float spacingY, bool isFood)
    {
        base.HandleTakeTakeable(takeable, type, spacingY, isFood);

        OnStackMaxed(IsStackMax);
    }

    protected override ITakeable HandleGiveTakeable()
    {
        OnStackMaxed(IsStackMax);

        return base.HandleGiveTakeable();
    }

    private void HandleStackMaxed(bool isStackMax)
    {
        if (isStackMax)
            StartCoroutine(StackMaxUIRoutine());
        else
            _stackMaxText.enabled = isStackMax;
    }

    private IEnumerator StackMaxUIRoutine()
    {
        yield return new WaitUntil(() => _stackComponent.IsObJumped);

        Vector3 movePos = _stackComponent.TopObjPos + new Vector3(0, 1, 0);
        _stackMaxText.transform.position = movePos;
        _stackMaxText.enabled = true;
    }

    private void HandleOnGetPaid(int moneyAmount)
    {
        // 돈 받았을 때 이벤트 처리 해주기
        CoinManager.Instance.Coin += (int)(moneyAmount * _sellingCostWeigth);

        // Sound
        SoundManager.Instance.Play(AudioClips.Money, 1);

        // UI Update
    }

    private int HandleOnPaidCost(int cost)
    {
        // 돈 냈을 때 이벤트 처리 해주기

        // UI Update

        return 0;
    }

    private void HandlePriceChangingEvent(Transform moveTrm)
    {
        StartCoroutine(PriceChangingRoutine(moveTrm));
    }
    #endregion

    public void SetPlayerStat(float weight, float speed, int stack)
    {
        SetSellingCost(weight);
        _agentMovement.SetMoveSpeed(speed);
        _stackComponent.SetMaxStackCount(stack);
    }
    private void SetSellingCost(float weight)
    {
        _sellingCostWeigth = weight;
    }
}

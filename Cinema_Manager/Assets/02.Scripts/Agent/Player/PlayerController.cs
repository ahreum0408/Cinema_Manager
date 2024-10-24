using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3;
using System;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static AyunDefine;

public class PlayerController : AgentController
{
    [Header("INPUT")]
    [SerializeField] private FloatingJoystick _joystick;

    [Header("UI")]
    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private TextMeshProUGUI _stackMaxText;

    // Component
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
            Camera mainCam = Camera.main;
            //// 월드 좌표를 스크린 좌표로 변환
            //Vector3 worldPos = _stackComponent.GetTopObjectTrm().position + new Vector3(0, 10, 0);
            //Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);

            //RectTransformUtility.ScreenPointToLocalPointInRectangle(
            //    _playerCanvas.GetComponent<RectTransform>(),  // 캔버스의 RectTransform
            //    screenPos,                             // 스크린 좌표
            //    null,                                  // Screen Space - Overlay일 때는 null
            //    out Vector2 localPos                           // 변환된 로컬 좌표
            //);

            //// 변환된 좌표를 UI Text의 위치로 설정
            //_stackMaxText.rectTransform.anchoredPosition = new Vector2(0, localPos.y);

            // 스택의 맨 위 오브젝트의 월드 포지션을 화면 좌표로 변환
            Vector3 worldPosition = _stackComponent.GetTopObjectTrm().position;
            worldPosition.x = 0;
            Vector3 screenPosition = mainCam.WorldToScreenPoint(worldPosition);
            // Max 텍스트 UI의 위치를 화면 좌표로 설정
            _stackMaxText.rectTransform.anchoredPosition = screenPosition;
        }
    }


    private void HandleOnGetPaid(int moneyAmount)
    {
        // 돈 받았을 때 이벤트 처리 해주기
        CoinManager.Instance.Coin += moneyAmount;
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

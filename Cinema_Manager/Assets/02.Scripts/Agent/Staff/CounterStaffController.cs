using DG.Tweening;
using UnityEngine;

public class CounterStaffController : AgentController, IOpenTarget
{
    // Component
    private AgentAnimationComponent _agentAnimation;

    [SerializeField] private TargetType _targetType;
    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

    private bool _isFirst;
    public bool IsFirst { get => _isFirst; set => _isFirst = value; }

    public void ActiveObj(bool active, bool on = false)
    {
        _isOpen = active;
        gameObject.SetActive(active);

        ScaleSetting();
        LevelEvents.ChangeCounterStaffActiveEvent?.Invoke(this, active);
    }

    public void ScaleSetting()
    {
        float time = 0.5f;
        Vector3 originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, time).SetEase(Ease.OutBack);
    }

    protected override void Init()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = transform.Find("Visual").GetComponent<Animator>();

        // 계산 모션 넣어주기
    }
    protected override void SetAgentComponents()
    {
        base.SetAgentComponents();

        _agentAnimation = GetAgentComponent<AgentAnimationComponent>();
    }
}

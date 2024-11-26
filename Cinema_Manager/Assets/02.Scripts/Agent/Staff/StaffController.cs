using UnityEngine;
using UnityEngine.AI;

public class StaffController : AgentController
{
    [HideInInspector] public bool IsStacked => StackCompo.IsStacked;

    // Object
    [HideInInspector] public Table table;
    [HideInInspector] public DisplayStand displayStand;
    [HideInInspector] public FoodContainer foodContainer;

    [HideInInspector] public Transform restPos; // 작업 없을 때 직원이 있을 곳

    // Components
    public NavMeshAgent Agent { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    private AgentState _currentState;

    public bool IsMoving => Agent.velocity.sqrMagnitude > 0.1f;

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }

    protected override void SetAgentComponents()
    {
        base.SetAgentComponents();

        AnimationCompo = GetAgentComponent<AgentAnimationComponent>();
        StackCompo = GetAgentComponent<AgentStackComponent>();
    }

    private void Start()
    {
        restPos = transform;
        ChangeState(new IdleState(this));

        OnTakeTakeable += HandleTakeTakeable;
        OnGiveTakeable += HandleGiveTakeable;
    }

    private void Update()
    {
        _currentState?.Update();
        SetMoveAnimation();
    }

    private void SetMoveAnimation()
    {
        if (IsMoving)
            AnimationCompo.SetMovementAnimation(Agent.destination);
        else
            AnimationCompo.SetMovementAnimation(Vector3.zero);
    }

    public bool CanSetDestination()
    {
        return Vector3.Distance(Agent.destination, transform.position) < Agent.stoppingDistance;
    }

    public void ChangeState(AgentState newState)
    {
        if (_currentState != newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}

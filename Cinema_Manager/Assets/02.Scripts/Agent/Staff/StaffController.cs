using System;
using UnityEngine;
using UnityEngine.AI;
using static AyunDefine;

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

    private Collider collider;
    private AgentState _currentState;

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
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
        SetMoveAniamtion();
        CheckCanStack();
    }

    private void SetMoveAniamtion()
    {
        if (Agent.velocity.sqrMagnitude > 0)
            AnimationCompo.SetMovementAnimation(Agent.destination);
        else
            AnimationCompo.SetMovementAnimation(Vector3.zero);
    }

    private void CheckCanStack()
    {
        if (!CanSetDestination())
            collider.enabled = false;
        else
            collider.enabled = true;
    }

    public bool CanSetDestination()
    {
        float threshold = Agent.stoppingDistance + 0.1f;
        if (!Agent.isPathStale && Agent.remainingDistance < threshold)
            return true;
        else
            return false;
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

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

    public Transform restPos;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    private AgentState _currentState;

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
        ChangeState(new IdleState(this));

        OnTakeTakeable += HandleTakeTakeable;
        OnGiveTakeable += HandleGiveTakeable;
    }

    private void Update()
    {
        _currentState?.Update();
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

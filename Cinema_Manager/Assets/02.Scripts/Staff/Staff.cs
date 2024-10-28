using BehaviorDesigner.Runtime;
using System;
using UnityEngine;
using static AyunDefine;
using UnityEngine.AI;

public class Staff : AgentController
{
    public float defualtSpeed = 3.5f;

    [SerializeField] private LayerMask _whatIsPlayer;

    [HideInInspector] public bool IsStacked => StackCompo.IsStacked;
    [HideInInspector] public Vector3 startPos;

    // Components
    public NavMeshAgent Agent { get; private set; }
    public AgentStackComponent StackCompo { get; private set; }
    public AgentAnimationComponent AnimationCompo { get; private set; }

    // Events
    public Action<ITakeable, PoolableType, float, bool> OnTakeTakeable;
    public Func<ITakeable> OnGiveTakeable;

    protected override void Init()
    {
        Animator = GetComponentInChildren<Animator>();

        Agent = GetComponent<NavMeshAgent>();
        StackCompo = GetComponent<AgentStackComponent>();
        AnimationCompo = GetComponent<AgentAnimationComponent>();
    }

    private void Start()
    {
        Agent.speed = defualtSpeed;
        startPos = transform.position;

        OnTakeTakeable += HandleTakeTakeable;
        OnGiveTakeable += HandleGiveTakeable;
    }

    public bool CanSetDestination()
    {
        float distance = Vector3.Distance(transform.position, Agent.destination);
        if (distance == 0)
            return true;
        else
            return false;
    }

    #region Handle

    private void HandleTakeTakeable(ITakeable takeable, PoolableType type, float spacingY, bool isFood)
    {
        if (IsStacked == false)
            AnimationCompo.UpperHoldingAnimation(true);
        StackCompo.TakeObject(takeable, type, spacingY, isFood);
    }

    private ITakeable HandleGiveTakeable()
    {
        ITakeable takeable = StackCompo.GetTopObject();

        if (IsStacked == false)
            AnimationCompo.UpperHoldingAnimation(false);

        return takeable;
    }

    #endregion
}

public class SharedStaff : SharedVariable<Staff>
{
    public static implicit operator SharedStaff(Staff value)
    {
        return new SharedStaff { Value = value };
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public abstract class AgentController : MonoBehaviour
{
    protected List<AgentComponent> agentComponentList;

    // Stack Events
    public Action<ITakeable, PoolableType, float, bool> OnTakeTakeable;
    public Func<ITakeable> OnGiveTakeable;

    public event Action OnEnableEvent;
    public event Action OnUpdateEvent;
    public event Action OnFixedUpdateEvent;
    public event Action OnDisableEvent;

    public Rigidbody Rigidbody { get; protected set; }
    public Animator Animator { get; protected set; }

    private void Awake()
    {
        Init();
        SetAgentComponents();
    }

    // Rigidbody, Animator 등 공용 변수 세팅
    protected virtual void Init() { }

    // AgentComponent 세팅
    protected virtual void SetAgentComponents()
    {
        agentComponentList = new List<AgentComponent>();
        GetComponentsInChildren(agentComponentList);

        foreach (AgentComponent component in agentComponentList)
        {
            component.Init(this);
        }
    }

    public T GetAgentComponent<T>() where T : AgentComponent
    {
        AgentComponent agentComponent = agentComponentList.Find(component => component is T);

        if (agentComponent == null)
        {
            Debug.LogError($"Not Exist {typeof(T).Name}");
        }

        return agentComponent as T;
    }

    #region Event

    protected virtual void OnEnable()
    {
        OnEnableEvent?.Invoke();
    }

    private void Update()
    {
        OnUpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdateEvent?.Invoke();
    }

    protected virtual void OnDisable()
    {
        OnDisableEvent?.Invoke();
    }

    #endregion

    #region Handle
    public bool CanTakeFood(PoolableType type)
    {
        AgentStackComponent stackCompo = GetAgentComponent<AgentStackComponent>();
        bool isSameType = stackCompo.CurrentHoldType == PoolableType.None
            || stackCompo.CurrentHoldType == type;

        return isSameType && !stackCompo.IsStackMax;
    }

    protected virtual void HandleTakeTakeable(ITakeable takeable, PoolableType type, float spacingY, bool isFood)
    {
        AgentStackComponent stackCompo = GetAgentComponent<AgentStackComponent>();
        if (stackCompo.IsStacked == false)
            GetAgentComponent<AgentAnimationComponent>().UpperHoldingAnimation(true);
        stackCompo.TakeObject(takeable, type, spacingY, isFood);
    }

    public bool CanGiveTakeable(PoolableType type)
    {
        AgentStackComponent stackCompo = GetAgentComponent<AgentStackComponent>();
        return stackCompo.CurrentHoldType == type && stackCompo.IsStacked;
    }

    protected virtual ITakeable HandleGiveTakeable()
    {
        AgentStackComponent stackCompo = GetAgentComponent<AgentStackComponent>();
        ITakeable takeable = stackCompo.GetTopObject();

        if (stackCompo.IsStacked == false)
            GetAgentComponent<AgentAnimationComponent>().UpperHoldingAnimation(false);

        return takeable;
    }
    #endregion
}

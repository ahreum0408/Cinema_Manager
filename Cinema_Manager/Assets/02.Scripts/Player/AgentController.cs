using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AgentController : MonoBehaviour
{
    protected List<AgentComponent> agentComponentList;

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

    protected T GetAgentComponent<T>() where T : AgentComponent
    {
        AgentComponent agentComponent = agentComponentList.Find(component => component is T);

        if (agentComponent == null)
        {
            Debug.LogError($"Not Exist {typeof(T).Name}");
        }

        return agentComponent as T;
    }
}

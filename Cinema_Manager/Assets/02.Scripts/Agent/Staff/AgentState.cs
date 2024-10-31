using UnityEngine.AI;

public abstract class AgentState
{
    protected StaffController agent;
    protected NavMeshAgent navAgent;

    public AgentState(StaffController agent)
    {
        this.agent = agent;
        this.navAgent = agent.GetComponent<NavMeshAgent>();
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

using UnityEngine.AI;

public abstract class AgentState
{
    protected StaffController agent;
    protected NavMeshAgent navAgent;

    public AgentState(StaffController agent)
    {
        this.agent = agent;
        this.navAgent = agent.Agent;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

using UnityEngine;

public class MoveToTargetState : AgentState
{
    private Vector3 targetPosition;
    private AgentState nextState;

    public MoveToTargetState(StaffController agent, Vector3 targetPosition, AgentState nextState)
        : base(agent)
    {
        this.targetPosition = targetPosition;
        this.nextState = nextState;
    }

    public override void Enter()
    {
        navAgent.SetDestination(targetPosition);
    }

    public override void Update()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            agent.ChangeState(nextState);
        }
    }

    public override void Exit()
    {
    }
}

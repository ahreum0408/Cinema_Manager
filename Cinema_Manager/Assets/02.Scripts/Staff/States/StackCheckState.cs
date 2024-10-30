using UnityEngine;

internal class StackCheckState : AgentState
{
    public StackCheckState(StaffController agent) : base(agent)
    {
    }

    public override void Enter()
    {
        agent.AnimationCompo.SetMovementAnimation(Vector3.zero);
    }

    public override void Update()
    {
        if(!agent.IsStacked)
            agent.ChangeState(new MoveToTargetState(agent, agent.restPos.position, new IdleState(agent)));
    }

    public override void Exit()
    {
    }
}
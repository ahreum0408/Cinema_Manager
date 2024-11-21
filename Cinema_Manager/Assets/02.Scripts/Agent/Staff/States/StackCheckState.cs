internal class StackCheckState : AgentState
{
    public StackCheckState(StaffController agent) : base(agent)
    {
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (!agent.IsStacked)
        {
            if (agent.displayStand != null)
            {
                agent.displayStand.IsWorking = false;
                agent.displayStand = null;
            }

            agent.ChangeState(new MoveToTargetState(agent, agent.restPos.position, new IdleState(agent)));
        }
    }

    public override void Exit()
    {
    }
}
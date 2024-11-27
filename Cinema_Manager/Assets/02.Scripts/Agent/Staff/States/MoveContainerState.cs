internal class MoveContainerState : AgentState
{
    public MoveContainerState(StaffController agent) : base(agent)
    {
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (CheckFoodContainer())
            agent.ChangeState(new MoveToTargetState
                (agent, agent.displayStand.staffPoint.transform.position, new StackCheckState(agent)));
    }

    public override void Exit()
    {
    }

    private bool CheckFoodContainer()
    {
        if (agent.IsStacked)
        {
            agent.foodContainer = null;
            return true;
        }
        return false;
    }
}
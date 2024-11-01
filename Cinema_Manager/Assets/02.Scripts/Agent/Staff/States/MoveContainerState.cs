using UnityEngine;

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
        if(CheckFoodContainer())
            agent.ChangeState(new MoveToTargetState
                (agent, agent.displayStand.staffPoint.transform.position, new StackCheckState(agent)));
    }

    public override void Exit()
    {
    }

    private bool CheckFoodContainer()
    {
        if((agent.foodContainer.currentFoodCnt == 0 && agent.IsStacked) || agent.StackCompo.IsStackMax)
            return true;
        return false;
    }
}
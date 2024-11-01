using UnityEngine;

internal class CounterState : AgentState
{
    public CounterState(StaffController agent) : base(agent)
    {
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (CheckCounter())
        {
            agent.ChangeState(new MoveToTargetState(agent, agent.restPos.position, new IdleState(agent)));
        }
    }

    public override void Exit()
    {

    }

    private bool CheckCounter()
    {
        Counter counter = ObjectManager.Instance.counter;
        return counter.lineList.Count == 0;
    }
}
using UnityEngine;

public class CleanTableState : AgentState
{
    public CleanTableState(StaffController agent) : base(agent) { }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (IsCleaningComplete())
        {
            TrashBin trashBin = ObjectManager.Instance.trashBin;
            Vector3 trashBinPos = trashBin.staffPoint.transform.position;

            agent.ChangeState(new MoveToTargetState(agent, trashBinPos, new StackCheckState(agent)));
        }
    }

    public override void Exit()
    {
    }

    private bool IsCleaningComplete()
    {
        if (agent.table.FindDirtyChair() == null || agent.StackCompo.RemainingStackCount == 0)
        {
            agent.table.IsWorking = false;
            agent.table = null;
            return true;
        }
        return false;
    }
}

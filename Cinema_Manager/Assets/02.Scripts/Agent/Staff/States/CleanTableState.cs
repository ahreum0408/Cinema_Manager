using TMPro;
using UnityEngine;

public class CleanTableState : AgentState
{
    public CleanTableState(StaffController agent) : base(agent) { }

    public override void Enter()
    {
        agent.AnimationCompo.SetMovementAnimation(Vector3.zero);
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
        if (agent.table.FindDirtyChair() == null)
        {
            agent.table = null;
            return true;
        }
        return false;
    }
}

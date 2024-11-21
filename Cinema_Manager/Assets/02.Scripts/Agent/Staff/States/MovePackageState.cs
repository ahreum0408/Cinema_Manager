using UnityEngine;

internal class MovePackageState : AgentState
{
    public MovePackageState(StaffController agent) : base(agent)
    {
    }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (CheckPackage())
        {
            ObjectManager.Instance.parcelService.IsWorking = false;
            Vector3 parcelPos = ObjectManager.Instance.boxContainer.staffPoint.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, parcelPos, new IdleState(agent)));
        }
    }

    public override void Exit()
    {
    }

    private bool CheckPackage()
    {
        ParcelService parcelService = ObjectManager.Instance.parcelService;
        return parcelService.CurrentBoxCnt == 0;
    }
}
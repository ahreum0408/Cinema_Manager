using UnityEngine;

public class IdleState : AgentState
{
    public IdleState(StaffController agent) : base(agent) { }

    public override void Enter()
    {
        agent.AnimationCompo.SetMovementAnimation(Vector3.zero);

        navAgent.isStopped = true;

        agent.table = CheckTable();
        agent.displayStand = CheckDisplay();
    }

    public override void Update()
    {
        if (agent.table != null)
        {
            Vector3 tablePosition = agent.table.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, tablePosition, new CleanTableState(agent)));
        }
        else if (CheckDisplay() != null)
        {
            agent.ChangeState(new MoveFoodState(agent));
        }
        else if (CheckCounter())
        {
            agent.ChangeState(new CounterState(agent));
        }
        else if (CheckPackage())
        {
            agent.ChangeState(new MovePackageState(agent));
        }
    }

    public override void Exit()
    {
    }

    private Table CheckTable() 
    { 
        foreach(Table table in ObjectManager.Instance.tables)
        {
            Point point = table.FindDirtyChair();
            if (point != null)
                return table;
        }
        return null; 
    }

    private DisplayStand CheckDisplay() 
    {
        foreach (DisplayStand displayStand in ObjectManager.Instance.displayStands)
        {
            if (displayStand.CurrentLine > 0)
                return displayStand;
        }
        return null;
    }

    private bool CheckCounter() 
    { 
        Counter counter = ObjectManager.Instance.counter;
        if (counter.lineList.Count > 0 && !counter.IsInteraction)
            return true;
        
        return false; 
    }
    private bool CheckPackage() 
    { 
        ParcelService parcelService = ObjectManager.Instance.parcelService;
        if(parcelService.CurrentBoxCnt > 0 && !parcelService.IsInteraction)
            return true;
        return false; 
    }
}

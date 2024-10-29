using UnityEngine;

public class IdleState : AgentState
{
    public IdleState(StaffController agent) : base(agent) { }

    public override void Enter()
    {
        agent.AnimationCompo.SetMovementAnimation(Vector3.zero);

        navAgent.isStopped = true;

    }

    public override void Update()
    {
        agent.table = CheckTable();
        agent.displayStand = CheckDisplay();
        if (agent.table != null)
        {
            Debug.Log("1");

            Vector3 tablePosition = agent.table.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, tablePosition, new CleanTableState(agent)));
        }
        //else if (CheckDisplay() != null)
        //{
        //    Debug.Log("2");

        //    agent.ChangeState(new MoveFoodState(agent));
        //}
        else if (CheckCounter())
        {
            Debug.Log("3");

            Vector3 counterPos = ObjectManager.Instance.counter.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, counterPos, new CounterState(agent)));
        }
        else if (CheckPackage())
        {
            Debug.Log("4");

            Vector3 parcelPos = ObjectManager.Instance.parcelService.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, parcelPos, new CounterState(agent)));
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
        return counter.lineList.Count > 0 && !counter.IsInteraction;
    }

    private bool CheckPackage()
    {
        ParcelService parcelService = ObjectManager.Instance.parcelService;
        return parcelService.CurrentBoxCnt > 0 && !parcelService.IsInteraction;
    }
}

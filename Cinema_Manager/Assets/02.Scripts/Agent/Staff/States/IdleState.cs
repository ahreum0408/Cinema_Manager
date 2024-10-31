using UnityEngine;

public class IdleState : AgentState
{
    public IdleState(StaffController agent) : base(agent) { }

    public override void Enter()
    {
        navAgent.isStopped = true;
    }

    public override void Update()
    {
        agent.table = CheckTable();
        agent.displayStand = CheckDisplay();
        if (agent.table != null)
        {
            Vector3 tablePos = agent.table.staffPoint.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, tablePos, new CleanTableState(agent)));
        }
        else if (CheckDisplay() != null)
        {
            agent.foodContainer = FindFoodContainer();
            agent.ChangeState(new MoveToTargetState
                (agent, agent.foodContainer.staffPoint.transform.position, new MoveContainerState(agent)));
        }
        else if (CheckCounter())
        {
            Vector3 counterPos = ObjectManager.Instance.counter.staffPoint.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, counterPos, new CounterState(agent)));
        }
        else if (CheckPackage())
        {
            Vector3 parcelPos = ObjectManager.Instance.parcelService.staffPoint.transform.position;
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

    private FoodContainer FindFoodContainer()
    {
        foreach(FoodContainer foodContainer in ObjectManager.Instance.foodContainers)
        {
            if(foodContainer.GetPoolObjType() == agent.displayStand.GetPoolObjType())
                return foodContainer;
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

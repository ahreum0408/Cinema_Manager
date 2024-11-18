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
        else if (agent.displayStand != null)
        {
            agent.foodContainer = FindFoodContainer();
            if (agent.foodContainer != null)
            {
                agent.ChangeState(new MoveToTargetState
                    (agent, agent.foodContainer.staffPoint.transform.position, new MoveContainerState(agent)));
            }
        }
        //else if (CheckCounter())
        //{
        //    Vector3 counterPos = ObjectManager.Instance.counter.staffPoint.transform.position;
        //    agent.ChangeState(new MoveToTargetState(agent, counterPos, new CounterState(agent)));
        //}
        else if (CheckPackage())
        {
            Vector3 parcelPos = ObjectManager.Instance.parcelService.staffPoint.transform.position;
            agent.ChangeState(new MoveToTargetState(agent, parcelPos, new MovePackageState(agent)));
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
            if (point != null && !table.IsWorking)
            {
                table.IsWorking = true;
                return table;
            }
        }
        return null; 
    }

    private DisplayStand CheckDisplay() 
    {
        foreach (DisplayStand displayStand in ObjectManager.Instance.displayStands)
        {
            if (displayStand.gameObject.active && displayStand.CurrentLine > 0
                && !displayStand.IsWorking && 
                agent.StackCompo.MaxStackCount <= displayStand.StackMaxCnt - displayStand.GetFoodStack())
            {
                displayStand.IsWorking = true;
                return displayStand;
            }
        }
        return null;
    }

    private FoodContainer FindFoodContainer()
    {
        foreach(FoodContainer foodContainer in ObjectManager.Instance.foodContainers)
        {
            if(foodContainer.gameObject.active && foodContainer.GetPoolObjType() == agent.displayStand.GetPoolObjType())
                return foodContainer;
        }
        return null;
    }

    private bool CheckCounter()
    {
        Counter counter = ObjectManager.Instance.counter;

        if (counter.IsWorking)
            return false;

        counter.IsWorking = true;
        return counter.lineList.Count > 0 && !counter.IsInteraction;
    }

    private bool CheckPackage()
    {
        ParcelService parcelService = ObjectManager.Instance.parcelService;

        if (!ObjectManager.Instance.parcelService.gameObject.active || parcelService.IsWorking) 
            return false;

        parcelService.IsWorking = true;
        return parcelService.CurrentBoxCnt > 0 && !parcelService.IsInteraction;
    }
}

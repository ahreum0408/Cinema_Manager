using BehaviorDesigner.Runtime.Tasks;

public class CheckBuy : Conditional
{
    public SharedCustomer customer;
    public SharedCounter counter;

    public override TaskStatus OnUpdate()
    {
        if (counter.Value.lineList.Count == 1)
        {
            customer.Value.ChangeState(CustomerState.Buy);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}

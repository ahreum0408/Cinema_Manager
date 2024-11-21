using BehaviorDesigner.Runtime.Tasks;

public class CheckSeat : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.customerData.isSeat && ObjectManager.Instance.CanUseTable() != null)
        {
            customer.Value.currentChair = ObjectManager.Instance.CanUseTable().CanSeatChair();
            customer.Value.currentChair.ChangeUsingState(true);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}

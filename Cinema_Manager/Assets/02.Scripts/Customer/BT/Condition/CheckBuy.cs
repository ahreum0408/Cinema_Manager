using BehaviorDesigner.Runtime.Tasks;

// 구매 가능한 상태인지 체크
public class CheckBuy : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.customerData.isBuy == true && customer.Value.CanSetDestination()
            && !ObjectManager.Instance.counter.moneyDummy.IsAddMoney)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}

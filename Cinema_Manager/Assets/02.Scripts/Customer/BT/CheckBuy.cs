using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// 구매 가능한 상태인지 체크
public class CheckBuy : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Basic && customer.Value.isBad == true)
            return TaskStatus.Failure;

        if (customer.Value.isBuy == true)
            return TaskStatus.Success;
        else
            return TaskStatus.Running;
    }
}

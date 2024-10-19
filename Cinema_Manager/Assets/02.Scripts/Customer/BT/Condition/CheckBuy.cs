using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// 구매 가능한 상태인지 체크
public class CheckBuy : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call && customer.Value.customerData.isBad == true)
        {
            customer.Value.AnimationCompo.CallAnimation(1);
            return TaskStatus.Success;
        }

        if (customer.Value.customerData.isBuy == true && customer.Value.CanSetDestination())
        {
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Running;
    }
}

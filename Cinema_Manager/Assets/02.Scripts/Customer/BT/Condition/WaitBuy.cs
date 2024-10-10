using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// 구매 대기
public class WaitBuy : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.customerData.isCalculate)
        {
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }
}

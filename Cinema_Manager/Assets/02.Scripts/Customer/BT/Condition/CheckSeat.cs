using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSeat : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        CustomerSpawnManager.Instance.MinusCustomer();
        if(customer.Value.customerData.isSeat && ObjectManager.Instance.CanUseTable() != null)
        {
            customer.Value.currentChair = ObjectManager.Instance.CanUseTable().CanSeatChair();
            customer.Value.currentChair.ChangeUsingState(true);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}

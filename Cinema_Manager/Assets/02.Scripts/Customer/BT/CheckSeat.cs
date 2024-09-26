using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSeat : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if(customer.Value.isSeat && customer.Value.Table.CanSeatChair() != null)
        {
            customer.Value.currentChair = customer.Value.Table.CanSeatChair();
            customer.Value.currentChair.ChangeUsingState(true);
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Failure;
    }
}

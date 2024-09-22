using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSeat : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if(customer.Value.isSeat && customer.Value.CanSeatChair() != null)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;
    }
}

using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFood : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if(customer.Value.customerData.isGive && customer.Value.CanSetDestination())
            customer.Value.currentStand.GiveFood();

        if (customer.Value.StackCompo.RemainingStackCount == 0)
        {
            customer.Value.currentStand.RemoveCustomer(customer.Value);
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }
}

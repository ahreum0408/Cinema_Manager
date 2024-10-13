using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFood : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.StackCompo.RemainingStackCount == 0)
        {
            return TaskStatus.Failure;
        }
        else
        {
            customer.Value.currentStand.GiveFood();
            return TaskStatus.Running;
        }
    }
}

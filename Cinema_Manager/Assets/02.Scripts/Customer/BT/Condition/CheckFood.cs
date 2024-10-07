using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFood : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.customerData.isGet == true)
        {
            return TaskStatus.Success;
        }
        else
            return TaskStatus.Running;
    }
}

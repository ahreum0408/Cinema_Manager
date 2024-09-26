using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCustomerType : Conditional
{
    public SharedCustomer customer;

    public override TaskStatus OnUpdate()
    {
        if(customer.Value.CurrentCustomerType == CustomerType.Basic) 
            return TaskStatus.Success;
        
        return TaskStatus.Failure;
    }
}

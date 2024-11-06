using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndCustomer : Action
{
    public SharedCustomer customer;

    private Vector3 _destination;
    public override void OnStart()
    {
        _destination = customer.Value.startPos;
        customer.Value.Agent.SetDestination(_destination);
    }

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.CanSetDestination())
        {
            PoolManager.Instance.Push(customer.Value.CurrentCustomerType.ToString() + "Customer", customer.Value.gameObject);
            return TaskStatus.Failure;
        }
        return TaskStatus.Running;
    }
}

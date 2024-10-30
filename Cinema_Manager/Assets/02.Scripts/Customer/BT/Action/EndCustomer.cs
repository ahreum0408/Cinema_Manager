using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EndCustomer : Action
{
    public SharedCustomer customer;

    private Vector3 _destination;
    private NavMeshAgent _agent;

    public override void OnAwake()
    {
        _agent = customer.Value.Agent;
    }

    public override void OnStart()
    {
        _destination = customer.Value.startPos;
        _agent.SetDestination(_destination);
    }

    public override TaskStatus OnUpdate()
    {
        customer.Value.AnimationCompo.SetMovementAnimation(_destination);

        float threshold = _agent.stoppingDistance + 0.1f;
        if (!_agent.isPathStale && _agent.remainingDistance < threshold)
        {
            PoolManager.Instance.Push
                (customer.Value.CurrentCustomerType.ToString() + "Customer", customer.Value.gameObject);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}

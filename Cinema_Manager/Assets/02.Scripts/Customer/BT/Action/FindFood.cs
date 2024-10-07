using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class FindFood : Action
{
    public SharedCustomer customer;

    private DisplayStand stand;

    private Vector3 _destination;
    private NavMeshAgent _agent;

    private bool _isStarted;

    public override void OnAwake()
    {
        _agent = customer.Value.Agent;
    }

    public override void OnStart()
    {
        stand = ObjectManager.Instance.FindDisplayStand(customer.Value.customerData.objectType);
        stand.AddCustomer(customer.Value);
        customer.Value.currentStand = stand;

        _destination = customer.Value.Agent.destination;
        _agent.SetDestination(_destination);
        _isStarted = true;
    }

    public override TaskStatus OnUpdate()
    {
        customer.Value.AnimationCompo.SetMovementAnimation(_destination);

        if (_isStarted)
        {
            _isStarted = false;
            return TaskStatus.Running;
        }

        float threshold = _agent.stoppingDistance + 0.1f;
        if (!_agent.isPathStale && _agent.remainingDistance < threshold)
        {
            customer.Value.AnimationCompo.SetMovementAnimation(Vector3.zero);
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}

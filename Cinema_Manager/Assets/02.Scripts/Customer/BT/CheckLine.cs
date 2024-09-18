using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CheckLine : Action
{
    public SharedCustomer customer;
    public SharedCounter counter;

    private Vector3 _destination;
    private NavMeshAgent _agent;

    private bool _isStarted;

    public override void OnAwake()
    {
        _agent = customer.Value.Agent;
    }

    public override void OnStart()
    {
        counter.Value.AddCustomer(customer.Value);
        _destination = counter.Value.checkPoint.position;
        _agent.SetDestination(_destination);
        _isStarted = true;
        Debug.Log(_destination);
    }

    public override TaskStatus OnUpdate()
    {
        if (_isStarted)
        {
            _isStarted = false;
            return TaskStatus.Running;
        }

        float threshold = _agent.stoppingDistance + 0.1f;
        if (!_agent.isPathStale && _agent.remainingDistance < threshold)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}

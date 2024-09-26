using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

// 구매 줄 체크 & 이동
public class CheckLine : Action
{
    public SharedCustomer customer;

    private Vector3 _destination;
    private NavMeshAgent _agent;

    private bool _isStarted;

    public override void OnAwake()
    {
        _agent = customer.Value.Agent;
    }

    public override void OnStart()
    {
        customer.Value.Counter.AddCustomer(customer.Value);
        _destination = customer.Value.Counter.checkPoint.position;
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
            return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}

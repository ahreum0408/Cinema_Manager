using BehaviorDesigner.Runtime.Tasks;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.AI;

// 구매 줄 체크 & 이동
public class FindLine : Action
{
    public SharedCustomer customer;

    private Vector3 _destination;
    private NavMeshAgent _agent;

    private bool _isStarted;
    private float startTime;

    public float clearTime;

    public override void OnAwake()
    {
        _agent = customer.Value.Agent;
    }

    public override void OnStart()
    {
        if(customer.Value.CurrentCustomerType == CustomerType.Parcel)
        {
            ObjectManager.Instance.parcelService.AddCustomer(customer.Value);
            _destination = ObjectManager.Instance.parcelService.checkPoint.position;
        }
        else
        {
            ObjectManager.Instance.counter.AddCustomer(customer.Value);
            _destination = ObjectManager.Instance.counter.checkPoint.position;
        }
        
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
            if(customer.Value.CurrentCustomerType == CustomerType.Call)
            {
                customer.Value.Agent.speed = 0;
                customer.Value.AnimationCompo.CallAnimation(1);

                if (customer.Value.CheckPlayer())
                {
                    startTime += Time.deltaTime;
                    if (clearTime <= startTime)
                    {
                        customer.Value.AnimationCompo.SleepAnimation(-1);
                        customer.Value.CurrentCustomerType = CustomerType.Basic;
                        return TaskStatus.Success;
                    }
                }
                else
                {
                    if (startTime >= 0)
                        startTime -= Time.deltaTime;
                }
            }
            else
                return TaskStatus.Success;
        }
        return TaskStatus.Running;
    }
}

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EndCustomer : Action
{
    public SharedCustomer customer;

    private Vector3 _destination;
    public override void OnStart()
    {
        _destination =
            new Vector3(customer.Value.startPos.x, customer.Value.startPos.y, customer.Value.startPos.z - 4);
        customer.Value.Agent.SetDestination(_destination);
    }

    public override TaskStatus OnUpdate()
    {
        if (Vector3.Distance(
            customer.Value.Agent.destination, transform.position) <
            customer.Value.Agent.stoppingDistance + 0.5f)
        {
            PoolManager.Instance.Push(customer.Value.CurrentCustomerType.ToString() + "Customer", customer.Value.gameObject);
            return TaskStatus.Failure;
        }
        return TaskStatus.Running;
    }
}

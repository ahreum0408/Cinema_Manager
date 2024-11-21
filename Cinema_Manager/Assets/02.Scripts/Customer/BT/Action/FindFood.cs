using BehaviorDesigner.Runtime.Tasks;

public class FindFood : Action
{
    public SharedCustomer customer;

    private bool _isStarted;

    public override void OnStart()
    {
        customer.Value.currentStand.AddCustomer(customer.Value);
        _isStarted = true;
    }

    public override TaskStatus OnUpdate()
    {
        if (_isStarted)
        {
            _isStarted = false;
            return TaskStatus.Running;
        }

        if (customer.Value.CanSetDestination())
            return TaskStatus.Success;

        return TaskStatus.Running;
    }
}

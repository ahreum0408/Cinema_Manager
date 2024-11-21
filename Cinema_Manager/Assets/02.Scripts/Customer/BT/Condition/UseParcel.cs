using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UseParcel : Conditional
{
    public SharedCustomer customer;
    public float parcelTime;

    private float startTime;

    public override void OnStart()
    {
        customer.Value.AnimationCompo.PackAnimation(1);
        startTime = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        if (parcelTime <= Time.time - startTime)
        {
            ObjectManager.Instance.parcelService.BoxSpawn();
            customer.Value.AnimationCompo.PackAnimation(-1);
            ObjectManager.Instance.parcelService.RemoveCustomer(customer.Value);
            return TaskStatus.Failure;
        }
        return TaskStatus.Running;
    }
}

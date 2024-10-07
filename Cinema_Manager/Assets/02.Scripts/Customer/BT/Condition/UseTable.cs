using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UseTable : Conditional
{
    public SharedCustomer customer;
    public float eatTime; // 하나 먹는데 걸리는 시간

    private float lastEatTime = 0;
    private int currentEat;

    public override void OnStart()
    {
        customer.Value.AnimationCompo.SeatAnimation(1);
        lastEatTime = Time.time;
        currentEat = customer.Value.customerData.wantBuy;
    }

    public override TaskStatus OnUpdate()
    {
        if (currentEat == 0)
        {
            if(currentEat == 0 && customer.Value.CurrentCustomerType == CustomerType.Sleep)
                return TaskStatus.Running;

            customer.Value.currentChair.ChangeUsingState(false);
            customer.Value.currentChair.ChangeDirtyState(true);

            customer.Value.AnimationCompo.SeatAnimation(-1);
            return TaskStatus.Failure;
        }

        if (eatTime <= Time.time - lastEatTime)
        {
            currentEat--;
            lastEatTime = Time.time;
        }
        return TaskStatus.Running;
    }

}

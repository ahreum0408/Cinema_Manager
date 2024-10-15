using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class UseTable : Conditional
{
    public SharedCustomer customer;
    public float eatTime; // 하나 먹는데 걸리는 시간

    private float startEatTime = 0;
    private int currentEat; // 현재 먹은 음식 갯수

    public float clearTime; // 진상 퇴치하는데 걸리는 시간
    private float startTime;

    public override void OnStart()
    {
        customer.Value.AnimationCompo.SeatAnimation(1);
        startEatTime = Time.time;
        //currentEat = customer.Value.customerData.wantBuy;
    }

    public override TaskStatus OnUpdate()
    {
        if (currentEat == 0)
        {
            if(customer.Value.CurrentCustomerType == CustomerType.Sleep)
            {
                customer.Value.AnimationCompo.SleepAnimation(1);

                if (customer.Value.CheckPlayer())
                {
                    startTime = Time.time;
                    if(clearTime <= Time.time - startTime)
                    {
                        customer.Value.AnimationCompo.SleepAnimation(-1);
                        customer.Value.CurrentCustomerType = CustomerType.Basic;
                    }
                }
                return TaskStatus.Running;
            }

            customer.Value.currentChair.ChangeUsingState(false);
            customer.Value.currentChair.ChangeDirtyState(true);

            customer.Value.AnimationCompo.SeatAnimation(-1);
            return TaskStatus.Failure;
        }

        if (eatTime <= Time.time - startEatTime)
        {
            currentEat--;
            startEatTime = Time.time;
        }
        return TaskStatus.Running;
    }

}

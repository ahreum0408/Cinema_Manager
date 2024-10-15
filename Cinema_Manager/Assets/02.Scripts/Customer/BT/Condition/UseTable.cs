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
        currentEat = customer.Value.StackCompo.CurrentStackCount;
    }

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.StackCompo.CurrentStackCount > 0)
            customer.Value.currentChair.TakeFood(customer.Value, 1f);

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
            }
            else
            {
                customer.Value.currentChair.ChangeUsingState(false);
                customer.Value.currentChair.ChangeDirtyState(true);

                customer.Value.AnimationCompo.SeatAnimation(-1);
                return TaskStatus.Failure;
            }
        }

        if (eatTime <= Time.time - startEatTime)
        {
            currentEat--;
            customer.Value.currentChair.EatFood();
            startEatTime = Time.time;
        }
        return TaskStatus.Running;
    }

}

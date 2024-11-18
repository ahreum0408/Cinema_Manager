using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using static AyunDefine;

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
        if(customer.Value.IsStacked)
            customer.Value.currentChair.TakeFood(customer.Value, customer.Value.SpacingY, customer.Value.IsFood);

        if (currentEat == 0)
        {
            if(customer.Value.CurrentCustomerType == CustomerType.Sleep)
            {
                customer.Value.SetCanvas(true);

                customer.Value.AnimationCompo.SeatAnimation(-1);
                customer.Value.AnimationCompo.SleepAnimation(1);

                if (customer.Value.CheckPlayer())
                {
                    startTime += Time.deltaTime;
                    customer.Value.SetGauge(startTime / clearTime);
                    if (clearTime <= startTime)
                    {
                        customer.Value.SetCanvas(false);

                        customer.Value.AnimationCompo.SleepAnimation(-1);
                        customer.Value.CurrentCustomerType = CustomerType.Basic;
                        CustomerSpawnManager.Instance.MinusBadCustomer();
                    }
                }
                else
                {
                    if(startTime >= 0)
                        startTime -= Time.deltaTime;
                    customer.Value.SetGauge(startTime / clearTime);
                }
            }
            else
            {
                customer.Value.currentChair.ChangeUsingState(false);
                customer.Value.currentChair.ChangeDirtyState(true);

                customer.Value.AnimationCompo.SeatAnimation(-1);

                customer.Value.currentChair.trash =
                    PoolManager.Instance.Pop(PoolableType.Trash.ToString(),customer.Value.currentChair.holder);

                customer.Value.currentChair.GetComponentInParent<Table>().AddMoney();
                return TaskStatus.Failure;
            }
            return TaskStatus.Running;
        }

        if (eatTime <= Time.time - startEatTime)
        {
            currentEat--;
            startEatTime = Time.time;
            customer.Value.currentChair.EatFood();
        }
        return TaskStatus.Running;
    }

}

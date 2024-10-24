using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

// 구매 가능한 상태인지 체크
public class CheckBuy : Conditional
{
    public SharedCustomer customer;

    public float clearTime; // 진상 퇴치하는데 걸리는 시간
    private float startTime;

    public override void OnStart()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call)
        {
            customer.Value.AnimationCompo.UpperHoldingAnimation(false);
            customer.Value.AnimationCompo.CallAnimation(1);
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call)
        {
            if (customer.Value.CheckPlayer())
            {
                startTime += Time.deltaTime;
                if (clearTime <= startTime)
                {
                    customer.Value.AnimationCompo.CallAnimation(-1);
                    customer.Value.AnimationCompo.UpperHoldingAnimation(true);

                    ObjectManager.Instance.counter.SettingLine();

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

        if (customer.Value.customerData.isBuy == true && customer.Value.CanSetDestination())
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}

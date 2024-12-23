using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using static AyunDefine;

public class CheckFood : Conditional
{
    public SharedCustomer customer;

    public float clearTime;
    private float startTime;

    private bool isCustomerStop = false;
    private bool isStartBad = true;
    private bool isStartGive = true;

    private GameObject _soundObj;

    public override void OnStart()
    {
        isStartGive = true;
    }

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call
            && customer.Value.currentStand.GetCustomerIndex(customer.Value) == 0)
        {
            if(isStartBad)
            {
                // Play Sound
                _soundObj = SoundManager.Instance.Play(AudioClips.CallCustomer, true, 1.5f, customer.Value.transform, true);

                StopCustomers();
                customer.Value.SetBadCanvas(true);
                customer.Value.AnimationCompo.CallAnimation(1);

                isStartBad = false;
            }

            if (customer.Value.CheckPlayer())
            {
                startTime += Time.deltaTime;
                customer.Value.SetGauge(startTime / clearTime);
                if (clearTime <= startTime)
                {
                    customer.Value.SetBadCanvas(false);

                    customer.Value.AnimationCompo.CallAnimation(-1);
                    customer.Value.CurrentCustomerType = CustomerType.Basic;

                    // Stop Sound
                    SoundManager.Instance.PushSoundObj(_soundObj);
                    SoundManager.Instance.Play(AudioClips.BuyObject, 1f);

                    ResumeCustomers();
                }
            }
            else
            {
                if (startTime >= 0)
                    startTime -= Time.deltaTime;
                customer.Value.SetGauge(startTime / clearTime);
            }
            return TaskStatus.Running;
        }

        if (!isCustomerStop && 
            customer.Value.currentStand.GetCustomerIndex(customer.Value) == 0)
        {
            if(isStartGive)
            {
                customer.Value.SetFoodCanvas(true);
                isStartGive = false;
            }
            customer.Value.currentStand.GiveFood();
        }

        if (customer.Value.StackCompo.RemainingStackCount == 0
            && ObjectManager.Instance.counter.IsCanStand)
        {
            customer.Value.SetFoodCanvas(false);
            customer.Value.currentStand.RemoveCustomer(customer.Value);
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }

    private void StopCustomers()
    {
        isCustomerStop = true;

        int currentIndex = customer.Value.currentStand.GetCustomerIndex(customer.Value);
        var customers = customer.Value.currentStand.GetAllCustomers();

        for (int i = currentIndex; i < customers.Count; i++)
        {
            if (customers[i].CanSetDestination())
            {
                customers[i].Agent.isStopped = true;
            }
        }
    }

    private void ResumeCustomers()
    {
        isCustomerStop = false;

        int currentIndex = customer.Value.currentStand.GetCustomerIndex(customer.Value);
        var customers = customer.Value.currentStand.GetAllCustomers();

        for (int i = 0; i < customers.Count; i++)
        {
            customers[i].Agent.isStopped = false;
        }
    }
}

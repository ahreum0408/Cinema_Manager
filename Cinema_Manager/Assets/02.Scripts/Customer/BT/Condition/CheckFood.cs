using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFood : Conditional
{
    public SharedCustomer customer;

    public float clearTime;
    private float startTime;

    public override void OnStart()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call)
        {
            customer.Value.AnimationCompo.CallAnimation(1);
            StopCustomersBehind();
        }
    }

    private void StopCustomersBehind()
    {
        int currentIndex = customer.Value.currentStand.GetCustomerIndex(customer.Value);
        if (currentIndex == -1) return;

        var customers = customer.Value.currentStand.GetAllCustomers();
        for (int i = currentIndex + 1; i < customers.Count; i++)
        {
            customers[i].Agent.isStopped = true;
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
                    customer.Value.CurrentCustomerType = CustomerType.Basic;

                    ResumeCustomersBehind();
                    return TaskStatus.Success;
                }
            }
            else
            {
                if (startTime >= 0)
                    startTime -= Time.deltaTime;
            }
        }

        if (customer.Value.customerData.isGive && customer.Value.CanSetDestination())
            customer.Value.currentStand.GiveFood();

        if (customer.Value.StackCompo.RemainingStackCount == 0)
        {
            customer.Value.currentStand.RemoveCustomer(customer.Value);
            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }

    private void ResumeCustomersBehind()
    {
        int currentIndex = customer.Value.currentStand.GetCustomerIndex(customer.Value);
        if (currentIndex == -1) return;

        var customers = customer.Value.currentStand.GetAllCustomers();
        for (int i = currentIndex + 1; i < customers.Count; i++)
        {
            customers[i].Agent.isStopped = false;
        }
    }

}

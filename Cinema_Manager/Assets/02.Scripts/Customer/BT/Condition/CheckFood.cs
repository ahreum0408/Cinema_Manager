using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFood : Conditional
{
    public SharedCustomer customer;

    public float clearTime;
    private float startTime;
    private bool isCustomerStop = false;

    public override void OnStart()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call)
        {
            customer.Value.AnimationCompo.CallAnimation(1);
            StopCustomers();
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

                    ResumeCustomers();
                }
            }
            else
            {
                if (startTime >= 0)
                    startTime -= Time.deltaTime;
            }
            return TaskStatus.Running;
        }

        if (customer.Value.CanSetDestination())
        {
            if (!isCustomerStop && customer.Value.customerData.isGive)
                customer.Value.currentStand.GiveFood();

            if (customer.Value.StackCompo.RemainingStackCount == 0)
            {
                customer.Value.currentStand.RemoveCustomer(customer.Value);
                return TaskStatus.Failure;
            }
        }

        return TaskStatus.Running;
    }

    private void StopCustomers()
    {
        int currentIndex = customer.Value.currentStand.GetCustomerIndex(customer.Value);
        var customers = customer.Value.currentStand.GetAllCustomers();

        for (int i = currentIndex; i < customers.Count; i++)
        {
            customers[i].Agent.isStopped = true;
        }

        isCustomerStop = true;
    }

    private void ResumeCustomers()
    {
        int currentIndex = customer.Value.currentStand.GetCustomerIndex(customer.Value);
        if (currentIndex == -1) return;

        var customers = customer.Value.currentStand.GetAllCustomers();

        for (int i = currentIndex; i < customers.Count; i++)
        {
            customers[i].Agent.isStopped = false;
        }

        isCustomerStop = false;
    }
}

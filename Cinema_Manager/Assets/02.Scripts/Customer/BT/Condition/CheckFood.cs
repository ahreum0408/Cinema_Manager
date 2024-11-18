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
            customer.Value.AnimationCompo.CallAnimation(1);
    }

    public override TaskStatus OnUpdate()
    {
        if (customer.Value.CurrentCustomerType == CustomerType.Call)
        {
            StopCustomers();
            customer.Value.SetCanvas(true);

            if (customer.Value.CheckPlayer())
            {
                startTime += Time.deltaTime;
                customer.Value.SetGauge(startTime / clearTime);
                if (clearTime <= startTime)
                {
                    customer.Value.SetCanvas(false);

                    customer.Value.AnimationCompo.CallAnimation(-1);
                    customer.Value.CurrentCustomerType = CustomerType.Basic;
                    CustomerSpawnManager.Instance.MinusBadCustomer();

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

        if (!isCustomerStop && customer.Value.customerData.isGive)
            customer.Value.currentStand.GiveFood();

        if (customer.Value.StackCompo.RemainingStackCount == 0
            && ObjectManager.Instance.counter.IsCanStand
            && customer.Value.currentStand.GetCustomerIndex(customer.Value) == 0)
        {
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

        for (int i = currentIndex; i < customers.Count; i++)
        {
            customers[i].Agent.isStopped = false;
        }
    }
}

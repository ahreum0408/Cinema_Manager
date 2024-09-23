using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private float lineInterval;
    public Transform checkPoint;

    public List<Customer> lineList = new List<Customer>();

    private bool isStart = true; // √π º’¥‘¿Œ∞°?

    public void AddCustomer(Customer customer)
    {
        lineList.Add(customer);

        if (isStart)
        {
            customer.isBuy = true;
            isStart = false;
        }
        else
        {
            checkPoint.position = new Vector3(
                checkPoint.position.x,
                checkPoint.position.y,
                checkPoint.position.z - lineInterval
            );
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        lineList.Remove(customer);

        isStart = true;
        Customer beforeCustomer = null;
        foreach (var customers in lineList)
        {
            if(isStart)
            {
                customers.isBuy = true;
                isStart = false;
            }

            if(beforeCustomer == null)
            {
                customers.Agent.SetDestination(new Vector3(
                    customers.Agent.destination.x,
                    customers.Agent.destination.y,
                    customers.Agent.destination.z + lineInterval)
                );
            }
            else
            {
                customers.Agent.SetDestination(new Vector3(
                    beforeCustomer.Agent.destination.x,
                    beforeCustomer.Agent.destination.y,
                    beforeCustomer.Agent.destination.z - lineInterval)
                );
            }
            beforeCustomer = customers;
        }
    }
}

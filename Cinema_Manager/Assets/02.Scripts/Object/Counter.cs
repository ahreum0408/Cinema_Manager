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
        foreach (var customers in lineList)
        {
            if(isStart)
            {
                customers.isBuy = true;
                isStart = false;
            }
            customers.Agent.SetDestination(new Vector3(
                customers.transform.position.x,
                customers.transform.position.y,
                customers.transform.position.z + lineInterval)
            );
            customers.Agent.destination
        }
    }
}

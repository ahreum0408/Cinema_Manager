using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private float lineInterval;
    public Transform checkPoint;

    public List<Customer> lineList = new List<Customer>();

    public void AddCustomer(Customer customer)
    {
        lineList.Add(customer);

        if (lineList.Count == 1)
            customer.ChangeState(CustomerState.Buy);
        else
        {
            customer.ChangeState(CustomerState.Line);
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

        if (customer.isSeat)
            customer.ChangeState(CustomerState.WaitSeat);
        else
            customer.ChangeState(CustomerState.End);

        foreach (var customers in lineList)
        {
            customers.Agent.SetDestination(new Vector3(
                customers.transform.position.x, 
                customers.transform.position.y, 
                customers.transform.position.z + lineInterval)
            );
        }
    }
}

public class SharedCounter : SharedVariable<Counter>
{
    public static implicit operator SharedCounter(Counter value)
    {
        return new SharedCounter { Value = value };
    }
}

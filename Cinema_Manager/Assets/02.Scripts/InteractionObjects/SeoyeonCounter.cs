using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class SeoyeonCounter : MonoBehaviour
{
    [SerializeField] private Transform objectHolder;
    [SerializeField] private float lineInterval;
    public Transform checkPoint;

    public List<Customer> lineList = new List<Customer>();

    private bool isStart = true; // √π º’¥‘¿Œ∞°?

    private void Start()
    {
        PoolManager.Instance.Pop(PoolableType.Bread.ToString(), Vector3.zero, Quaternion.identity);
    }

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

            if (customer.CurrentCustomerType == CustomerType.Call)
            {
                customer.ChangeCustomerMat();
                break;
            }

            if (beforeCustomer == null)
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

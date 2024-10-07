using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour, IIneractionable
{
    #region 서연
    [SerializeField] private Transform objectHolder;
    [SerializeField] private float lineInterval;
    public Transform checkPoint;

    public List<Customer> lineList = new List<Customer>();

    private bool isStart = true; // 첫 손님인가?
    #endregion

    private bool _isEnterInteraction = false;

    public void EnterInteraction()
    {
        Debug.Log("들어옴");
        _isEnterInteraction = true;
        StartCoroutine(CheckPayLoop());
    }

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        Debug.Log("나감");
    }

    // 지불 확인 작업 (플레이어가 카운터에 상호작용하고 있을 때만 실행)
    private IEnumerator CheckPayLoop()
    {
        while (_isEnterInteraction)
        {
            // 여기서 계산 하는거 해주면 됨
            yield return null;
        }
    }

    public void AddCustomer(Customer customer)
    {
        lineList.Add(customer);

        if (isStart)
        {
            customer.customerData.isBuy = true;
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
            if (isStart)
            {
                customers.customerData.isBuy = true;
                isStart = false;
            }

            if (customer.CurrentCustomerType == CustomerType.Call)
            {
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

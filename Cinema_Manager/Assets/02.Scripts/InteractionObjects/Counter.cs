using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Counter : MonoBehaviour, IIneractionable
{
    #region 서연
    public bool IsWorking = false;

    public Transform staffPoint;

    [SerializeField] private float lineInterval;
    public Transform checkPoint;

    [HideInInspector] public List<Customer> lineList = new List<Customer>();

    private bool isStart = true; // 첫 손님인가?
    [SerializeField] private int maxCustomer;

    public bool IsInteraction => _isEnterInteraction;
    public bool IsCanStand => lineList.Count <= maxCustomer;
    #endregion

    [HideInInspector] public GameObject GameObject => gameObject;
    private bool _isEnterInteraction = false;
    private bool _isCounterStaffStay = false;

    private MoneyDummy _moneyDummy;
    private NotifyImageComponent _notifyImageComponent;
    public MoneyDummy moneyDummy => _moneyDummy;

    private void Awake()
    {
        _moneyDummy = transform.GetComponentInChildren<MoneyDummy>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
    }

    private void Start()
    {
        checkPoint.position = new Vector3(
                checkPoint.position.x - lineInterval,
                checkPoint.position.y,
                checkPoint.position.z
            );
    }

    public void EnterInteraction(AgentController agent)
    {
        if (_isCounterStaffStay) return;

        if (agent as CounterStaffController != null)
            _isCounterStaffStay = true;

        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(CheckPayLoop());
    }

    public void ExitInteraction(AgentController agent)
    {
        if (_isCounterStaffStay) return;

        _isEnterInteraction = false;
        StopCoroutine(CheckPayLoop());
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    // 지불 확인 작업 (플레이어가 카운터에 상호작용하고 있을 때만 실행)
    private IEnumerator CheckPayLoop()
    {
        while (_isEnterInteraction)
        {
            yield return new WaitUntil(() => lineList.Count > 0);

            if (lineList[0].CanSetDestination())
                RemoveCustomer();

            yield return null;
        }
    }

    public void AddCustomer(Customer customer)
    {
        lineList.Add(customer);

        if (isStart || lineList.Count == 1)
        {
            customer.customerData.isBuy = true;
            isStart = false;
            checkPoint.position = new Vector3(
                checkPoint.position.x + lineInterval,
                checkPoint.position.y,
                checkPoint.position.z
            );
        }
        else
        {
            checkPoint.position = new Vector3(
                checkPoint.position.x + lineInterval,
                checkPoint.position.y,
                checkPoint.position.z
            );
        }
    }

    public void RemoveCustomer()
    {
        _moneyDummy.AddMoneyObject(3);

        lineList[0].customerData.isCalculate = true;
        lineList.Remove(lineList[0]);

        lineList = lineList.OrderBy(c => lineList).ToList();

        checkPoint.position = new Vector3(
                checkPoint.position.x - lineInterval,
                checkPoint.position.y,
                checkPoint.position.z
            );

        bool lineIsStart = true;
        Customer beforeCustomer = null;
        foreach (var customers in lineList)
        {
            if (lineIsStart)
            {
                customers.customerData.isBuy = true;
                lineIsStart = false;
            }
            
            if(beforeCustomer == null)
            {
                customers.Agent.SetDestination(new Vector3(
                    customers.Agent.destination.x - lineInterval,
                    customers.Agent.destination.y,
                    customers.Agent.destination.z)
                );
            }
            else
            {
                customers.Agent.SetDestination(new Vector3(
                    beforeCustomer.Agent.destination.x + lineInterval,
                    beforeCustomer.Agent.destination.y,
                    beforeCustomer.Agent.destination.z)
                );
            }
            beforeCustomer = customers;
        }
    }
}

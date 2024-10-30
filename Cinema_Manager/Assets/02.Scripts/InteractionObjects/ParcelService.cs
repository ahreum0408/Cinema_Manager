using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AyunDefine;

public class ParcelService : MonoBehaviour, IIneractionable
{
    #region 서연
    public Transform staffPoint;

    [SerializeField] private float lineInterval;
    public Transform checkPoint;

    public List<Customer> lineList = new List<Customer>();

    private bool isStart = true; // 첫 손님인가?

    public bool IsInteraction => _isEnterInteraction;

    #endregion

    private Stack<ITakeable> _boxStack;
    public int CurrentBoxCnt => _boxStack.Count;
    public int StackMaxCnt => _stackMaxCnt;

    [Header("Box")]
    [SerializeField] private Transform _spawnTrm;
    [SerializeField] private PoolableType _poolObjType;
    [Range(0, 5)][SerializeField] private float _spacingY;

    private bool _isEnterInteraction = false;

    private NotifyImageComponent _notifyImageComponent;


    #region 나중에 업그레이드로 빼야할 것들
    private int _stackMaxCnt = 8; // 스택에 쌓이는 음식 개수
    #endregion

    private void Awake()
    {
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _boxStack = new Stack<ITakeable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BoxSpawn();
        }
    }

    // 스택이 다 찼는지 확인
    public bool BoxStackCheck()
    {
        return CurrentBoxCnt < _stackMaxCnt;
    }

    // 택배 다 부치고 이거 실행
    public void BoxSpawn()
    {
        if (false == BoxStackCheck()) return;

        Vector3 spawnPos = Vector3.zero;
        spawnPos.y = _spacingY * CurrentBoxCnt;
        GameObject go = PoolManager.Instance.Pop(_poolObjType.ToString(), _spawnTrm, spawnPos, Quaternion.Euler(0, 0, 0));
        _boxStack.Push(go.GetComponent<ITakeable>());
    }

    public void EnterInteraction(Collider collider)
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(GetBoxRoutine(collider));
    }

    public void ExitInteraction(Collider collider)
    {
        _isEnterInteraction = false;
        StopCoroutine(GetBoxRoutine(collider));
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator GetBoxRoutine(Collider collider)
    {
        while (_isEnterInteraction)
        {
            if (CurrentBoxCnt > 0)
            {
                ITakeable takeable = _boxStack.Peek();

                // Player
                if (collider.TryGetComponent(out PlayerController player))
                {
                    if (player.CanGiveTakeable(_poolObjType))
                    {
                        player.OnTakeTakeable?.Invoke(_boxStack.Pop(), _poolObjType, _spacingY, false);
                        yield return new WaitForSeconds(0.15f);
                    }
                }
                // Staff
                else if (collider.TryGetComponent(out StaffController staff))
                {
                    if (staff.CanGiveTakeable(_poolObjType))
                    {
                        staff.OnTakeTakeable?.Invoke(_boxStack.Pop(), _poolObjType, _spacingY, false);
                        yield return new WaitForSeconds(0.15f);
                    }
                }
            }
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
                checkPoint.position.x + lineInterval,
                checkPoint.position.y,
                checkPoint.position.z
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

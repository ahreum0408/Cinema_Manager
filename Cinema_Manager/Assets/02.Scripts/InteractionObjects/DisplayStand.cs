using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class DisplayStand : MonoBehaviour, IIneractionable, IOpenTarget
{
    [HideInInspector] public GameObject GameObject => gameObject;
    public Transform staffPoint;

    private Stack<ITakeable> _foodStack;
    public int _currentFoodCnt => _foodStack.Count;
    public int StackMaxCnt => _spawnTrmList.Count * _columnSpawnCnt;
    public List<Point> points;

    [SerializeField] private PoolableType _poolObjType;
    [SerializeField] private int _columnSpawnCnt;
    [SerializeField] private List<Transform> _spawnTrmList = new List<Transform>();

    private bool _isOpen = false;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }

    #region 서연
    public bool IsFullLine => _customerDic.Count == points.Count;
    public int CurrentLine => _customerDic.Count;

    [Range(0, 5)][SerializeField] private float _spacingY;
    [Range(0, 5)][SerializeField] private float _spacingX;
    [SerializeField] private bool _isFood;

    private bool _isEnterInteraction = false;

    private NotifyImageComponent _notifyImageComponent;

    private Customer _currentCustomer;

    private Dictionary<Customer, int> _customerDic;
    private bool _isStart;
    private bool _dicIsStart;
    private int _customerCount = 0;
    #endregion

    private void Awake()
    {
        _customerDic = new Dictionary<Customer, int>();

        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _foodStack = new Stack<ITakeable>();

        points = GetComponentsInChildren<Point>().ToList();
    }

    private void Start()
    {
        _isStart = true;
    }

    public void ActiveObj(bool active, bool addCustomer = false) {
        _isOpen = active;
        gameObject.SetActive(active);
        LevelEvents.ChangeStandActiveEvent?.Invoke(this, active);
        if (active && addCustomer) { // 여기 문제 있을거임 주의**
            CustomerSpawnManager.Instance.SetMaxCustomer(GetPoolObjType());
        }
    }
    public void EnterInteraction(AgentController agent)
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.2f);
        StartCoroutine(TakeFoodRoutine(agent));
    }

    public void ExitInteraction(AgentController agent)
    {
        _notifyImageComponent.SetNotifySensorImage(1f);
        _isEnterInteraction = false;
        StopCoroutine(TakeFoodRoutine(agent));
    }

    private IEnumerator TakeFoodRoutine(AgentController agent)
    {
        while (_isEnterInteraction)
        {
            if (_foodStack.Count < StackMaxCnt)
            {
                if (agent.CanGiveTakeable(_poolObjType))
                {
                    ITakeable food = agent.OnGiveTakeable?.Invoke();
                    TakeFood(food);
                }
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void TakeFood(ITakeable food)
    {
        int col = _currentFoodCnt % _columnSpawnCnt;
        int row = _currentFoodCnt / _columnSpawnCnt;

        Vector3 foodPos = Vector3.zero;
        foodPos.x += (-_spacingX * col);

        food.Take(_spawnTrmList[row], foodPos, Vector3.zero);
        _foodStack.Push(food);

        LevelEvents.ChangeStandItemEvent?.Invoke(this, _currentFoodCnt);
    }

    public void GiveFood()
    {
        if (_currentFoodCnt > 0)
        {
            StartCoroutine(GiveFoodRoutine());
        }
    }

    private IEnumerator GiveFoodRoutine()
    {
        while (_currentFoodCnt > 0 && _currentCustomer != null)
        {
            if (_currentCustomer.StackCompo.RemainingStackCount != 0)
            {
                _currentCustomer.OnTakeTakeable?.Invoke(_foodStack.Pop(), _poolObjType, _spacingY, _isFood);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    // 서있을 곳이 있나?
    public Point CanStandPoint()
    {
        foreach (var point in points)
        {
            if (!point.IsUsing)
            {
                return point;
            }
        }
        return null;
    }

    public void AddCustomer(Customer customer)
    {
        if (_customerDic.Count >= points.Count)
            return;

        _customerDic.Add(customer, _customerCount);
        _customerCount++;

        if (_isStart)
        {
            _currentCustomer = customer;
            _currentCustomer.customerData.isGive = true;
            _isStart = false;
        }

        customer.Agent.SetDestination(points[_customerDic[customer]].transform.position);
    }

    public void RemoveCustomer(Customer customer)
    {
        if (_customerDic.ContainsKey(customer))
        {
            _customerDic.Remove(customer);
            _customerCount--;
        }

        _dicIsStart = true;

        List<Customer> customerKeys = new List<Customer>(_customerDic.Keys);

        for (int i = 0; i < customerKeys.Count; i++)
        {
            Customer currentCustomer = customerKeys[i];

            _customerDic[currentCustomer] = i;

            if (_dicIsStart)
            {
                _currentCustomer = currentCustomer;
                _currentCustomer.customerData.isGive = true;
                _dicIsStart = false;
            }

            currentCustomer.Agent.SetDestination(points[i].transform.position);
        }
    }

    public PoolableType GetPoolObjType() => _poolObjType;
    public int GetFoodStack() => _currentFoodCnt;

    public int GetCustomerIndex(Customer customer)
    {
        if (_customerDic.ContainsKey(customer))
        {
            return _customerDic[customer];
        }
        return -1;
    }

    public List<Customer> GetAllCustomers()
    {
        return _customerDic.Keys.ToList();
    }


    // 임시로 스텐드에 음식 채우는 함수
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddItemToStand(1);
        }
    }

    public void AddItemToStand(int addItemCount = 1)
    {
        for (int i = 0; i < addItemCount; i++)
        {
            GameObject go = PoolManager.Instance.Pop(_poolObjType.ToString(), transform);
            if (go.TryGetComponent(out ITakeable takeable))
                TakeFood(takeable);
        }
    }

}

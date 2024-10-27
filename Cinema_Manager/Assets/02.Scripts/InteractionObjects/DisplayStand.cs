using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static AyunDefine;

public class DisplayStand : MonoBehaviour, IIneractionable
{
    private Stack<ITakeable> _foodStack;
    public int _currentFoodCnt => _foodStack.Count;
    public int StackMaxCnt => _spawnTrmList.Count * _columnSpawnCnt;
    public List<Point> points;

    [SerializeField] private PoolableType _poolObjType;
    [SerializeField] private int _columnSpawnCnt;
    [SerializeField] private List<Transform> _spawnTrmList = new List<Transform>();

    #region 서연
    public bool IsFullLine => _customerDic.Count == points.Count;

    [Range(0, 5)][SerializeField] private float _spacingY;
    [Range(0, 5)] [SerializeField] private float _spacingX;
    [SerializeField] private bool _isFood;

    private bool _isEnterInteraction = false;

    private PlayerController _playerController;
    private NotifyImageComponent _notifyImageComponent;

    private Customer _currentCustomer;

    private Dictionary<Customer, int> _customerDic;
    private bool _isStart;
    private int _customerCount = 0;
    #endregion

    private void Awake()
    {
        _customerDic = new Dictionary<Customer, int>();

        _playerController = FindObjectOfType<PlayerController>(); // 나중에 싱글톤으로
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _foodStack = new Stack<ITakeable>();

        points = GetComponentsInChildren<Point>().ToList();
    }

    private void Start()
    {
        _isStart = true;
    }
    public void SetAvticeGameObject(bool active) {
        gameObject.SetActive(active);
    }
    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(TakeFoodRoutine());
    }

    public void ExitInteraction()
    {
        _notifyImageComponent.SetNotifySensorImage(1f);
        _isEnterInteraction = false;
        StopCoroutine(TakeFoodRoutine());
    }

    private IEnumerator TakeFoodRoutine()
    {
        while (_isEnterInteraction)
        {
            if (_playerController.CanGiveTakeable(_poolObjType) && _foodStack.Count < StackMaxCnt)
            {
                ITakeable food = _playerController.OnGiveTakeable?.Invoke();
                TakeFood(food);
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
        LevelEvents.ChangeDisplayStandEvent?.Invoke(this, _currentFoodCnt);
    }

    public void GiveFood()
    {
        if(_currentFoodCnt > 0)
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
        _customerDic.Remove(customer);
        _customerCount--;

        _isStart = true;

        List<Customer> customerKeys = new List<Customer>(_customerDic.Keys);

        for (int i = 0; i < customerKeys.Count; i++)
        {
            Customer currentCustomer = customerKeys[i];

            if (currentCustomer == customer)
            {
                _customerDic.Remove(currentCustomer);
                continue;
            }

            if (_customerDic.ContainsKey(currentCustomer))
            {
                _customerDic[currentCustomer] = _customerDic[currentCustomer] - 1;
            }

            if (_isStart)
            {
                _currentCustomer = currentCustomer;
                _currentCustomer.customerData.isGive = true;
                _isStart = false;
            }

            currentCustomer.Agent.SetDestination(points[_customerDic[currentCustomer]].transform.position);
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

}

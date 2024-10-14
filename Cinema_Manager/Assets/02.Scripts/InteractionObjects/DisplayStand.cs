using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static AyunDefine;

public class DisplayStand : MonoBehaviour, IIneractionable
{
    private Stack<ITakeable> _foodStack;
    private int _currentFoodCnt => _foodStack.Count;
    public int StackMaxCnt => _spawnTrmList.Count * _columnSpawnCnt;
    public List<Point> points;

    [SerializeField] private PoolableType _poolObjType;
    [SerializeField] private int _columnSpawnCnt;
    [SerializeField] private List<Transform> _spawnTrmList = new List<Transform>();

    [Range(0, 5)][SerializeField] private float _spacingY;
    [Range(0, 5)] [SerializeField] private float _spacingX;
    [SerializeField] private bool _isFood;

    private bool _isEnterInteraction = false;

    private PlayerController _playerController;
    private NotifyImageComponent _notifyImageComponent;

    private Customer _currentCustomer;

    private Dictionary<Customer, int> _customerDic;
    private bool _isStart;

    private void Awake()
    {
        _customerDic = new Dictionary<Customer, int>();

        _playerController = FindObjectOfType<PlayerController>(); // 나중에 싱글톤으로
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _foodStack = new Stack<ITakeable>();
    }

    private void Start()
    {
        _isStart = true;
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
            if (_playerController.CanGiveFood(_poolObjType) && _foodStack.Count < StackMaxCnt)
            {
                ITakeable food = _playerController.OnGiveFood?.Invoke();
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
    }

    public void GiveFood()
    {
        StartCoroutine(GiveFoodRoutine());
    }

    private IEnumerator GiveFoodRoutine()
    {
        while (_currentFoodCnt > 0 && _currentCustomer != null)
        {
            if (_currentCustomer.StackCompo.RemainingStackCount != 0)
            {
                _currentCustomer.OnTakeFood?.Invoke(_foodStack.Pop(), _poolObjType, _spacingY, _isFood);
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
        _customerDic.Add(customer, _customerDic.Count);
        if (_isStart)
        {
            _currentCustomer = customer;
            _isStart = false;
        }
        customer.Agent.SetDestination(points[_customerDic[customer]].transform.position);
    }

    public void RemoveCustomer(Customer customer)
    {
        _customerDic.Remove(customer);
        _isStart = true;
        foreach (var customers in _customerDic.Keys)
        {
            if (_isStart)
            {
                _currentCustomer = customers;
                _isStart = false;
            }
            customers.Agent.SetDestination(points[_customerDic[customer] -1].transform.position);
        }
        _currentCustomer = null;
    }

    public PoolableType GetPoolObjType() => _poolObjType;
    public int GetFoodStack() => _currentFoodCnt;
}

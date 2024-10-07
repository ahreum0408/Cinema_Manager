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
    [Range(0, 5)] [SerializeField] private float _spacingX;

    private bool _isEnterInteraction = false;

    private PlayerController _playerController;
    private NotifyImageComponent _notifyImageComponent;

    private Dictionary<Customer, int> _customerList;
    private bool _isStart = true;
    private int i;

    private void Awake()
    {
        i = 0;
        _customerList = new Dictionary<Customer, int>();

        _playerController = FindObjectOfType<PlayerController>(); // 나중에 싱글톤으로
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _foodStack = new Stack<ITakeable>();
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
        foodPos.x += (_spacingX * col);

        food.Take(_spawnTrmList[row], foodPos, Vector3.zero);
        _foodStack.Push(food);
    }

    private IEnumerator GiveBreadRoutine()
    {
        while (true)
        {
            // 여기서 손님에게 음식 줘야함
            yield return null;
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
        _customerList.Add(customer, i);
        i++;
        if (_isStart)
        {
            customer.customerData.isBuy = true;
            _isStart = false;
        }
        else
        {
            customer.Agent.SetDestination(points[_customerList[customer]].transform.position);
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        _customerList.Remove(customer);
        i--;
        _isStart = true;
        foreach (var customers in _customerList.Keys)
        {
            if (_isStart)
            {
                customers.customerData.isGet = true;
                _isStart = false;
            }

            customers.Agent.SetDestination(points[_customerList[customer]].transform.position);
        }
    }

    public PoolableType GetPoolObjType() => _poolObjType;
}
